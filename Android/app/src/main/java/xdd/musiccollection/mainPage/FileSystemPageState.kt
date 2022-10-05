package xdd.musiccollection.mainPage

import xdd.musiccollection.models.MainWindowViewState
import xdd.musiccollection.models.NodeModel

data class FileSystemPageState(
    var currentViewState: MainWindowViewState = MainWindowViewState.Browse,

    var isListLoading: Boolean = false,
    var isErrorLoading: Boolean = false,
    var filesList: List<NodeModel> = listOf(),
    var currentRoot: NodeModel? = null,
    var currentParent: NodeModel? = null,

    var isSearchOpened: Boolean = false,
    var searchQuery: String = ""
)