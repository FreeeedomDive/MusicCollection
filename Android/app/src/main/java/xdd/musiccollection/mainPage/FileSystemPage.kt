package xdd.musiccollection.mainPage

import android.util.Log
import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.LazyListState
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material.*
import androidx.compose.material.TabRowDefaults.Divider
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import xdd.musiccollection.defaultComponents.BackPressHandler
import xdd.musiccollection.defaultComponents.SearchComponent
import xdd.musiccollection.defaultComponents.TopAppBarSearchButton
import xdd.musiccollection.mainPage.viewModels.FileSystemPageViewModel
import xdd.musiccollection.models.MainWindowViewState
import xdd.musiccollection.models.NodeModel
import xdd.musiccollection.models.NodeType
import xdd.musiccollection.ui.theme.BlueColorPalette1
import xdd.musiccollection.ui.theme.BlueColorPalette3
import xdd.musiccollection.ui.theme.BlueColorPalette4

@Composable
fun FileSystemPage(viewModel: FileSystemPageViewModel = FileSystemPageViewModel()) {
    val currentContext = LocalContext.current
    val composableScope = rememberCoroutineScope()
    val (filteredCurrentItems, setFilteredItems) = remember { mutableStateOf(listOf<NodeModel>()) }
    val lazyListState: LazyListState = rememberLazyListState()
    val (isSearchOpened, setSearchOpened) = remember { mutableStateOf(false) }
    val (searchQuery, setSearchQuery) = remember { mutableStateOf("") }

    fun handleItemClick(element: NodeModel, disableSelectedElementLoading: () -> Unit) {
        when (element.type) {
            NodeType.File -> {
                composableScope.launch {
                    val tags = viewModel.loadFileTags(element)
                    if (!tags.isSuccess) {
                        when (tags.statusCode) {
                            404 -> Toast.makeText(currentContext, "File not found", Toast.LENGTH_LONG).show()
                            409 -> Toast.makeText(
                                currentContext,
                                "You are trying to get tags from directory",
                                Toast.LENGTH_LONG
                            ).show()
                            500 -> Toast.makeText(currentContext, "Failed request!", Toast.LENGTH_LONG).show()
                        }
                    } else if (tags.value == null){
                        Toast.makeText(currentContext, "No tags for this file", Toast.LENGTH_LONG).show()
                    } else {
                        val textLines = arrayOf(
                            "Artist: ${tags.value.artist}",
                            "Title: ${tags.value.trackName}",
                            "Album: ${tags.value.album}",
                            "Duration: ${tags.value.duration}",
                            "Format: ${tags.value.format}",
                            "Bit rate: ${tags.value.bitRate} kbps",
                            "Sample frequency: ${tags.value.sampleFrequency} Hz",
                            "Bits per sample: ${tags.value.bitDepth}",
                        )
                        Toast.makeText(currentContext, textLines.joinToString("\n"), Toast.LENGTH_LONG).show()
                    }
                    disableSelectedElementLoading()
                }
            }
            NodeType.Back -> {
                viewModel.loadPrevDirectory(disableSelectedElementLoading)
            }
            else -> {
                viewModel.loadNextDirectory(element, disableSelectedElementLoading)
            }
        }
    }

    when (viewModel.currentViewState) {
        MainWindowViewState.Settings -> BackPressHandler(true) {
            viewModel.setCurrentWindowViewState(MainWindowViewState.Browse)
        }
        MainWindowViewState.Browse -> if (viewModel.canGoBack()) {
            BackPressHandler(false) {}
        } else {
            BackPressHandler(true) {
                viewModel.loadPrevDirectory {}
            }
        }
        MainWindowViewState.Player -> BackPressHandler(true) {
            viewModel.setCurrentWindowViewState(MainWindowViewState.Browse)
        }
        else -> {
            viewModel.setCurrentWindowViewState(MainWindowViewState.Browse)
        }
    }
    Surface {
        Scaffold(
            topBar =
            {
                TopAppBar(
                    backgroundColor = BlueColorPalette1,
                    contentColor = Color.White
                ) {
                    if (isSearchOpened) {
                        SearchComponent(
                            searchQuery = searchQuery,
                            setSearchQuery = { query ->
                                Log.i("Search", "Query has changed to $query")
                                setSearchQuery(query)
                                setFilteredItems(viewModel.filesList.filter {
                                    if (query.isEmpty())
                                        true
                                    else it.path.contains(query, ignoreCase = true)
                                } as MutableList<NodeModel>)
                            },
                            onClose = { setSearchOpened(false) }
                        )
                    } else {
                        TopAppBarSearchButton(onClick = { setSearchOpened(true) })
                    }
                }
            })
        {
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .background(
                        brush = Brush.verticalGradient(
                            colors = listOf(
                                BlueColorPalette4,
                                BlueColorPalette3,
                            )
                        )
                    )
            )
            {
                if (viewModel.filesList.isEmpty()) {
                    Column(
                        Modifier
                            .fillMaxSize(),
                        verticalArrangement = Arrangement.Center
                    ) {
                        Text(
                            text = "The folder is empty...",
                            textAlign = TextAlign.Center,
                            modifier = Modifier
                                .fillMaxWidth()
                        )
                    }
                } else {
                    LazyColumn(
                        state = lazyListState
                    ) {
                        if (viewModel.isSearchOpened && viewModel.searchQuery.isNotEmpty()) {
                            itemsIndexed(filteredCurrentItems) { _, item ->
                                FileSystemListElement(element = item, handleItem = ::handleItemClick)
                                Divider(color = Color.Black)
                            }
                        } else {
                            itemsIndexed(viewModel.filesList) { _, item ->
                                FileSystemListElement(element = item, handleItem = ::handleItemClick)
                                Divider(color = Color.Black)
                            }
                        }
                    }
                }
            }
        }
    }
}