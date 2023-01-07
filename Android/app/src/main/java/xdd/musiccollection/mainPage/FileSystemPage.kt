package xdd.musiccollection.mainPage

import android.util.Log
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.material.*
import androidx.compose.material.TabRowDefaults.Divider
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.PlayArrow
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import xdd.musiccollection.R
import xdd.musiccollection.defaultComponents.BackPressHandler
import xdd.musiccollection.mainPage.viewModels.FileSystemPageViewModel
import xdd.musiccollection.models.MainWindowViewState
import xdd.musiccollection.models.NodeModel
import xdd.musiccollection.models.NodeType
import xdd.musiccollection.ui.theme.BlueColorPalette1
import xdd.musiccollection.ui.theme.BlueColorPalette3
import xdd.musiccollection.ui.theme.BlueColorPalette4
import java.util.*

@Composable
fun FileSystemPage(viewModel: FileSystemPageViewModel) {
    val dummy = NodeModel(UUID.randomUUID(), null, null, "", NodeType.Dummy, null, null)

    val composableScope = rememberCoroutineScope()
    val scaffoldState = rememberScaffoldState()

    fun handleItemClick(element: NodeModel, disableSelectedElementLoading: () -> Unit) {
        when (element.type) {
            NodeType.File -> {
                composableScope.launch {
                    val tags = viewModel.loadFileTags(element)
                    if (!tags.isSuccess) {
                        when (tags.statusCode) {
                            404 -> scaffoldState.snackbarHostState.showSnackbar(message = "File not found")
                            409 -> scaffoldState.snackbarHostState.showSnackbar(message = "You are trying to get tags from directory")
                            500 -> scaffoldState.snackbarHostState.showSnackbar(message = "Internal server is unavailable")
                        }
                    }
                    viewModel.setCurrentTrack(element)
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
                contentColor = Color.White,
                modifier = Modifier.fillMaxWidth()
            ) {
                Box(Modifier.height(32.dp)) {
                    Row(
                        Modifier.fillMaxSize(),
                        horizontalArrangement = Arrangement.Start,
                        verticalAlignment = Alignment.CenterVertically
                    ) {
                        Text(
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(start = 4.dp),
                            textAlign = TextAlign.Justify,
                            maxLines = 1,
                            text = viewModel.currentNode?.fileName() ?: "All folders"
                        )
                    }
                    Row(
                        Modifier.fillMaxSize(),
                        horizontalArrangement = Arrangement.End,
                        verticalAlignment = Alignment.CenterVertically
                    ) {
                        IconButton(
                            enabled = viewModel.currentNode != null,
                            onClick = {
                                composableScope.launch {
                                    scaffoldState.snackbarHostState.showSnackbar(
                                        message = "Creating queue for folder ${
                                            viewModel.currentNode?.fileName()
                                        }..."
                                    )
                                }
                                composableScope.launch {
                                    viewModel.createQueueFromDirectory()
                                }
                            }) {
                            Icon(
                                Icons.Default.PlayArrow,
                                contentDescription = "Play",
                                modifier = Modifier.size(24.dp)
                            )
                        }
                        IconButton(onClick = {
                            val newShuffleValue = !viewModel.shuffle
                            composableScope.launch {
                                scaffoldState.snackbarHostState.showSnackbar("Shuffle is ${if (newShuffleValue) "enabled" else "disabled"}\nReordering the queue...")
                            }
                            composableScope.launch {
                                viewModel.switchShuffle(newShuffleValue)
                            }
                        }) {
                            Icon(
                                painter = if (viewModel.shuffle)
                                    painterResource(id = R.drawable.shuffle_enabled)
                                else
                                    painterResource(id = R.drawable.shuffle_disabled),
                                contentDescription = "Shuffle",
                                modifier = Modifier.size(24.dp)
                            )
                        }
                        IconButton(onClick = {}) {
                            Icon(
                                painter = painterResource(id = R.drawable.show_queue),
                                contentDescription = "Show queue",
                                modifier = Modifier.size(24.dp)
                            )
                        }
                    }
                }
            }
        },
        bottomBar = {
            CurrentPlayingSongCard(viewModel)
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
                    itemsIndexed(viewModel.filesList + listOf(dummy)) { _, item ->
                        if (item.type == NodeType.Dummy) {
                            Column(Modifier.height(72.dp)) {}
                        } else {
                            FileSystemListElement(element = item, handleItem = ::handleItemClick)
                            Divider(color = Color.Black)
                        }
                    }
                }
            }
        }
    }
}