package xdd.musiccollection.mainPage.viewModels

import android.util.Log
import androidx.compose.foundation.lazy.LazyListState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.launch
import xdd.musiccollection.apiClient.FilesApiClient
import xdd.musiccollection.apiDto.Result
import xdd.musiccollection.extensions.convertToModel
import xdd.musiccollection.apiDto.music.AudioFileTagsDto
import xdd.musiccollection.models.MainWindowViewState
import xdd.musiccollection.models.NodeModel
import xdd.musiccollection.models.NodeType
import java.util.*

class FileSystemPageViewModel : ViewModel() {
    var currentViewState by mutableStateOf(MainWindowViewState.Browse)
        private set
    var isErrorLoading by mutableStateOf(false)
        private set
    var filesList by mutableStateOf(listOf<NodeModel>())
        private set
    var currentRoot by mutableStateOf<NodeModel?>(null)
        private set
    var isSearchOpened by mutableStateOf(false)
        private set
    var searchQuery by mutableStateOf("")
        private set
    var lazyListState by mutableStateOf(LazyListState())

    val songViewModel = SongViewModel()

    private var currentSkipValue by mutableStateOf(0)
    private var canLoadMore by mutableStateOf(true)
    private val take = 50

    private val filesApiClient = FilesApiClient()
    private val openedNodes: Stack<NodeModel> = Stack()

    init {
        loadRoots {}
    }

    fun search(query: String) {
        searchQuery = query
    }

    fun setOpenedSearch(value: Boolean) {
        isSearchOpened = value
    }

    suspend fun loadFileTags(node: NodeModel): Result<AudioFileTagsDto?> {
        val apiResult = filesApiClient.readNode(currentRoot!!.id, node.id)
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
        if (nextNode.type == NodeType.Root) {
            currentRoot = nextNode
        }
        openedNodes.push(nextNode)
        loadNode(nextNode, disableSelectedElementLoading)
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
            loadNode(lastOpenedNode.parent, disableSelectedElementLoading)
        }
    }

    private fun loadRoots(disableSelectedElementLoading: () -> Unit) {
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

    private fun loadNode(
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
        val apiResult = filesApiClient.readNodeAsDirectory(currentRoot!!.id, parent.id, currentSkipValue, take)
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
}