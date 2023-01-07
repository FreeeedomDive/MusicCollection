package xdd.musiccollection.mainPage.viewModels

import android.media.AudioAttributes
import android.media.MediaPlayer
import android.util.Log
import androidx.compose.foundation.lazy.LazyListState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.ResponseBody
import xdd.musiccollection.apiClient.FilesApiClient
import xdd.musiccollection.apiClient.QueuesApiClient
import xdd.musiccollection.apiDto.Result
import xdd.musiccollection.apiDto.music.AudioFileTagsDto
import xdd.musiccollection.extensions.convertToModel
import xdd.musiccollection.models.MainWindowViewState
import xdd.musiccollection.models.MoveQueuePositionResult
import xdd.musiccollection.models.NodeModel
import xdd.musiccollection.models.NodeType
import java.io.File
import java.io.FileInputStream
import java.io.FileOutputStream
import java.io.InputStream
import java.util.*

class FileSystemPageViewModel(private val userId: UUID, private val cacheDir: File) : ViewModel() {
    var currentViewState by mutableStateOf(MainWindowViewState.Browse)
        private set
    var isErrorLoading by mutableStateOf(false)
        private set
    var filesList by mutableStateOf(listOf<NodeModel>())
        private set
    var currentNode by mutableStateOf<NodeModel?>(null)
        private set
    var shuffle by mutableStateOf(false)
        private set
    var lazyListState by mutableStateOf(LazyListState())

    var currentSelectedTrack by mutableStateOf<NodeModel?>(null)
        private set

    private var mediaPlayer = MediaPlayer().apply {
        setAudioAttributes(
            AudioAttributes.Builder()
                .setContentType(AudioAttributes.CONTENT_TYPE_MUSIC)
                .setUsage(AudioAttributes.USAGE_MEDIA)
                .build()
        )
        setOnPreparedListener {
            resumePlaying()
        }
        setOnCompletionListener {
            viewModelScope.launch {
                val moveNextResult = moveNext()
                if (moveNextResult != MoveQueuePositionResult.Ok){
                    playingState = PlayingState.Pause
                }
            }
        }
    }

    var playingState by mutableStateOf(PlayingState.Pause)
        private set

    private var currentSkipValue by mutableStateOf(0)
    private var canLoadMore by mutableStateOf(true)
    private val take = 50

    private val queuesApiClient = QueuesApiClient()
    private val filesApiClient = FilesApiClient()
    private val openedNodes: Stack<NodeModel> = Stack()

    init {
        loadRoots {}
    }

    suspend fun createQueueFromDirectory() {
        val result = queuesApiClient.createQueue(userId, currentNode!!.id)
        if (!result.isSuccess) {
            Log.e("Create queue", result.statusCode.toString())
            return
        }
        val currentTrackResult = queuesApiClient.getCurrentTrack(userId)
        if (!currentTrackResult.isSuccess || currentTrackResult.value == null) {
            Log.e("Create queue and load current track", currentTrackResult.statusCode.toString())
            return
        }
        setCurrentTrack(currentTrackResult.value.track.convertToModel(currentNode))
    }

    suspend fun switchShuffle(newValue: Boolean) {
        val result = queuesApiClient.updateShuffle(userId, newValue)
        if (!result.isSuccess) {
            Log.e("Switch shuffle error status code", result.statusCode.toString())
            return
        }
        shuffle = newValue
    }

    suspend fun moveNext(): MoveQueuePositionResult? {
        val result = queuesApiClient.moveNext(userId)
        when (result.statusCode) {
            200 -> {
                setCurrentTrack(result.value!!.track.convertToModel(currentNode))
                return MoveQueuePositionResult.Ok
            }
            404 -> {
                return MoveQueuePositionResult.QueueNotExist
            }
            409 -> {
                return MoveQueuePositionResult.OutOfRange
            }
        }

        return null
    }

    suspend fun movePrevious(): MoveQueuePositionResult? {
        val result = queuesApiClient.movePrevious(userId)
        when (result.statusCode) {
            200 -> {
                setCurrentTrack(result.value!!.track.convertToModel(currentNode))
                return MoveQueuePositionResult.Ok
            }
            404 -> {
                return MoveQueuePositionResult.QueueNotExist
            }
            409 -> {
                return MoveQueuePositionResult.OutOfRange
            }
        }

        return null
    }

    suspend fun loadFileTags(node: NodeModel): Result<AudioFileTagsDto?> {
        val apiResult = filesApiClient.readNode(node.id)
        if (apiResult.isSuccess) {
            return Result.ok(apiResult.value!!.tags)
        }
        return Result.fail(apiResult.statusCode)
    }

    fun setCurrentWindowViewState(viewState: MainWindowViewState) {
        currentViewState = viewState
    }

    fun loadNextDirectory(
        nextNode: NodeModel,
        disableSelectedElementLoading: () -> Unit
    ) {
        openedNodes.push(nextNode)
        currentNode = nextNode
        loadFolder(nextNode, disableSelectedElementLoading)
    }

    fun loadPrevDirectory(
        disableSelectedElementLoading: () -> Unit = {}
    ) {
        if (openedNodes.empty()) {
            loadRoots(disableSelectedElementLoading)
            return
        }
        val lastOpenedNode = openedNodes.pop()
        if (lastOpenedNode.type == NodeType.Root || lastOpenedNode.parent == null) {
            loadRoots(disableSelectedElementLoading)
        } else {
            loadFolder(lastOpenedNode.parent, disableSelectedElementLoading)
        }
        if (!openedNodes.isEmpty()) {
            currentNode = openedNodes.peek()
        }
    }

    private fun loadRoots(disableSelectedElementLoading: () -> Unit) {
        currentNode = null
        Log.i("MainPage", "Start load roots")
        viewModelScope.launch {
            val rootsResult = filesApiClient.readAllRoots()
            if (rootsResult.isSuccess) {
                isErrorLoading = false
                filesList = rootsResult.value!!.map { x -> x.convertToModel() }
            } else {
                isErrorLoading = true
            }
            lazyListState.scrollToItem(0)
            disableSelectedElementLoading()
        }
    }

    fun loadMore() {
        viewModelScope.launch {
            if (!canLoadMore || openedNodes.empty()) {
                return@launch
            }
            currentSkipValue += take
            val nextNodes = loadItems(openedNodes.peek())
            canLoadMore = nextNodes.isNotEmpty()
            filesList = filesList + nextNodes
        }
    }

    private fun loadFolder(
        parent: NodeModel,
        disableSelectedElementLoading: () -> Unit = {}
    ) {
        viewModelScope.launch {
            currentSkipValue = 0
            canLoadMore = true
            isErrorLoading = false
            val nodes = loadItems(parent)
            disableSelectedElementLoading()
            filesList = listOf(
                NodeModel(
                    parent.id,
                    parent.parent,
                    parent.rootName,
                    parent.path,
                    NodeType.Back,
                    parent.directoryData,
                    parent.tags
                )
            ) + nodes
            lazyListState.scrollToItem(0)
        }
    }

    private suspend fun loadItems(parent: NodeModel): List<NodeModel> {
        val apiResult = filesApiClient.readNodeAsDirectory(parent.id, currentSkipValue, take)
        when (apiResult.statusCode) {
            200 -> {
                return apiResult.value!!.map { x -> x.convertToModel(parent) }
            }
            // actually impossible scenarios
            404 -> {
                isErrorLoading = true
                Log.e("FileSystemPage", "Node not found")
            }
            409 -> {
                isErrorLoading = true
                Log.e("FileSystemPage", "Node not found")
            }
            else -> {
                isErrorLoading = true
                Log.e("FileSystemPage", "Load node failed")
            }
        }
        return listOf()
    }

    fun resumePlaying() {
        playingState = PlayingState.Play
        mediaPlayer.start()
    }

    fun stopPlaying() {
        playingState = PlayingState.Pause
        if (mediaPlayer.isPlaying) {
            mediaPlayer.pause()
        }
    }

    private fun resetPlaying() {
        stopPlaying()
        mediaPlayer.reset()
    }

    suspend fun setCurrentTrack(track: NodeModel) {
        currentSelectedTrack = track
        resetPlaying()
        playingState = PlayingState.Loading
        val coroutineScope = CoroutineScope(Dispatchers.IO)
        coroutineScope.launch {
            val response = FilesApiClient().download(track.id)
            if (!response.isSuccess) {
                return@launch
            }
            val fileStream = saveFile(response.value, track) ?: return@launch
            mediaPlayer.apply {
                resetPlaying()
                setDataSource(fileStream.fd)
                try {
                    prepareAsync()
                } catch (e: IllegalStateException) {
                    e.printStackTrace()
                    reset()
                }
            }
        }
    }

    private fun saveFile(body: ResponseBody?, track: NodeModel): FileInputStream? {
        if (body == null)
            return null
        var input: InputStream? = null
        try {
            clearCache(cacheDir)
            input = body.byteStream()
            val file = File(cacheDir, buildFileName(track))
            val fos = FileOutputStream(file)
            fos.use { out ->
                input.copyTo(out)
            }
            return FileInputStream(file)
        } catch (e: Exception) {
            Log.e("saveFile exception", e.toString())
        } finally {
            input?.close()
        }
        return null
    }

    private fun clearCache(directory: File) {
        val filesInCache = directory.listFiles()
        filesInCache?.forEach { it.delete() }
    }

    private fun buildFileName(track: NodeModel): String {
        if (track.tags!!.artist != null && track.tags.trackName != null) {
            return "${track.tags.artist} - ${track.tags.trackName}.${track.tags.format}"
        }

        if (track.tags.trackName != null) {
            return "${track.tags.trackName}.${track.tags.format}"
        }

        return "${track.id}.${track.tags.format}"
    }
}