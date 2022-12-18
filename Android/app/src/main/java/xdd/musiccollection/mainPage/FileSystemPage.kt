package xdd.musiccollection.mainPage

import android.util.Log
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.material.*
import androidx.compose.material.TabRowDefaults.Divider
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.modifier.modifierLocalConsumer
import androidx.compose.ui.text.style.TextAlign
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
fun FileSystemPage(viewModel: FileSystemPageViewModel) {
    val composableScope = rememberCoroutineScope()
    val scaffoldState = rememberScaffoldState()

    fun handleItemClick(element: NodeModel, disableSelectedElementLoading: () -> Unit) {
        when (element.type) {
            NodeType.File -> {
                composableScope.launch {
                    val tags = viewModel.loadFileTags(element)
                    disableSelectedElementLoading()
                    if (!tags.isSuccess) {
                        when (tags.statusCode) {
                            404 -> scaffoldState.snackbarHostState.showSnackbar(message = "File not found")
                            409 -> scaffoldState.snackbarHostState.showSnackbar(message = "You are trying to get tags from directory")
                            500 -> scaffoldState.snackbarHostState.showSnackbar(message = "Internal server is unavailable")
                        }
                    }
                    viewModel.songViewModel.setCurrentTrack(element)
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

    if (viewModel.isErrorLoading) {
        LaunchedEffect(scaffoldState.snackbarHostState) {
            scaffoldState.snackbarHostState.showSnackbar(
                message = "Internal server is unavailable"
            )
        }
    }

    Log.i("Current ViewState", viewModel.currentViewState.toString())
    when (viewModel.currentViewState) {
        MainWindowViewState.Settings -> BackPressHandler(true) {
            viewModel.setCurrentWindowViewState(MainWindowViewState.Browse)
        }
        MainWindowViewState.Browse -> BackPressHandler(true) {
            viewModel.loadPrevDirectory {}
        }
        MainWindowViewState.Player -> BackPressHandler(true) {
            viewModel.setCurrentWindowViewState(MainWindowViewState.Browse)
        }
        MainWindowViewState.Queue -> BackPressHandler(true) {
            viewModel.setCurrentWindowViewState(MainWindowViewState.Player)
        }
        else -> {
            viewModel.setCurrentWindowViewState(MainWindowViewState.Browse)
        }
    }

    viewModel.lazyListState.OnBottomReached {
        viewModel.loadMore()
    }

    Scaffold(
        scaffoldState = scaffoldState,
        topBar =
        {
            TopAppBar(
                backgroundColor = BlueColorPalette1,
                contentColor = Color.White
            ) {
                if (viewModel.isSearchOpened) {
                    SearchComponent(
                        searchQuery = viewModel.searchQuery,
                        setSearchQuery = { query ->
                            Log.i("Search", "Query has changed to $query")
                            viewModel.search(query)
                        },
                        onClose = { viewModel.setOpenedSearch(false) }
                    )
                } else {
                    TopAppBarSearchButton(onClick = { viewModel.setOpenedSearch(true) })
                }
            }
        },
        bottomBar = {
            if (viewModel.songViewModel.currentSelectedTrack != null) {
                CurrentPlayingSongCard(viewModel.songViewModel)
            }
        }
    ) {
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
                    state = viewModel.lazyListState
                ) {
                    itemsIndexed(viewModel.filesList) { _, item ->
                        // Text(text = item.path, Modifier.clickable { handleItemClick(item) {} })
                        FileSystemListElement(element = item, handleItem = ::handleItemClick)
                        Divider(color = Color.Black)
                    }
                }
            }
        }
    }
}