package xdd.musiccollection.mainPage.viewModels

import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.launch
import xdd.musiccollection.apiClient.FilesApiClient
import xdd.musiccollection.apiDto.Result
import xdd.musiccollection.apiDto.files.convertToModel
import xdd.musiccollection.apiDto.music.AudioFileTagsDto
import xdd.musiccollection.mainPage.FileSystemPageState
import xdd.musiccollection.models.MainWindowViewState
import xdd.musiccollection.models.NodeModel
import xdd.musiccollection.models.NodeType
import java.util.*

class FileSystemPageViewModel : ViewModel() {
    var currentViewState by mutableStateOf(MainWindowViewState.Browse)
        private set
    var isListLoading by mutableStateOf(false)
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

    private val filesApiClient = FilesApiClient()
    private val openedNodes: Stack<NodeModel> = Stack()

    init {
        loadRoots {}
    }

    suspend fun loadFileTags(node: NodeModel): Result<AudioFileTagsDto?> {
        val apiResult = filesApiClient.readNode(currentRoot!!.id, node.id)
        if (apiResult.isSuccess) {
            return Result.ok(apiResult.value!!.tags)
        }
        return Result.fail(apiResult.statusCode)
    }

    fun canGoBack(): Boolean {
        return !openedNodes.empty()
    }

    fun setCurrentWindowViewState(viewState: MainWindowViewState) {
        currentViewState = viewState
    }

    fun loadNextDirectory(nextNode: NodeModel, disableSelectedElementLoading: () -> Unit) {
        if (nextNode.type == NodeType.Root) {
            currentRoot = nextNode
        }
        openedNodes.push(nextNode)
        loadNode(nextNode, disableSelectedElementLoading)
    }

    fun loadPrevDirectory(disableSelectedElementLoading: () -> Unit) {
        if (openedNodes.empty()) {
            loadRoots(disableSelectedElementLoading)
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
            isListLoading = true
            val rootsResult = filesApiClient.readAllRoots()
            if (rootsResult.isSuccess) {
                isErrorLoading = false
                filesList = rootsResult.value!!.map { x -> x.convertToModel() }
            } else {
                isErrorLoading = true
            }
            isListLoading = false
            disableSelectedElementLoading()
        }
    }

    private fun loadNode(parent: NodeModel, disableSelectedElementLoading: () -> Unit) {
        val elementsStackTrace = openedNodes.map { x -> "${x.id}      ${x.path}" }
        val str = elementsStackTrace.joinToString("\n")
        Log.i("FileSystemPageVM", "Current stack state: \n${str}")
        viewModelScope.launch {
            val apiResult = filesApiClient.readNodeAsDirectory(currentRoot!!.id, parent.id)
            when (apiResult.statusCode) {
                200 -> {
                    val nodes = apiResult.value!!.map { x -> x.convertToModel(parent) }
                    filesList = listOf(
                        NodeModel(
                            parent.id,
                            parent.parent,
                            parent.path,
                            NodeType.Back,
                            parent.tags
                        )
                    ) + nodes
                }
                // actually impossible scenarios
                404 -> {
                    Log.e("FileSystemPage", "Node not found")
                    // showErrorToast("Can't find this element")
                }
                409 -> {
                    Log.e("FileSystemPage", "Node not found")
                    // showErrorToast("You are trying to open file as a directory")
                }
                else -> {
                    Log.e("FileSystemPage", "Load node failed")
                    // showErrorToast()
                }
            }
            disableSelectedElementLoading()
        }
    }
}