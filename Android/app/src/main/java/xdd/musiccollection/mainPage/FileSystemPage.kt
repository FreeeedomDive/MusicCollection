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
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import xdd.musiccollection.defaultComponents.BackPressHandler
import xdd.musiccollection.helpers.Generator
import xdd.musiccollection.helpers.SearchComponent
import xdd.musiccollection.helpers.TopAppBarSearchButton
import xdd.musiccollection.models.FileSystemNode
import xdd.musiccollection.models.NodeType
import xdd.musiccollection.ui.theme.BlueColorPalette1
import xdd.musiccollection.ui.theme.BlueColorPalette3
import xdd.musiccollection.ui.theme.BlueColorPalette4

enum class CurrentViewState {
    Settings,
    Browse,
    Player,
}

@Composable
fun FileSystemPage() {
    val items = Generator().generateDirectories(null)
    val currentContext = LocalContext.current
    val (currentItems, setItems) = remember { mutableStateOf(items) }
    val (filteredCurrentItems, setFilteredItems) = remember { mutableStateOf(items) }
    val lazyListState: LazyListState = rememberLazyListState()
    val (isSearchOpened, setSearchOpened) = remember { mutableStateOf(false) }
    val (searchQuery, setSearchQuery) = remember { mutableStateOf("") }
    val (currentViewState, setCurrentViewState) = remember { mutableStateOf(CurrentViewState.Browse) }
    val (currentParent, setCurrentParent) = remember { mutableStateOf<FileSystemNode?>(null) }

    fun showNode(element: FileSystemNode?, regenerateItems: Boolean = true) {
        setCurrentParent(element)
        setItems(Generator().generateDirectories(element))
    }

    fun handleItemClick(element: FileSystemNode) {
        if (element.type == NodeType.Back) {
            showNode(element.parent)
        }
        if (element.type == NodeType.File) {
            Toast
                .makeText(currentContext, "This is file ${element.path}", Toast.LENGTH_LONG)
                .show()
        }
        if (element.type == NodeType.Directory) {
            showNode(element)
        }
    }

    when (currentViewState) {
        CurrentViewState.Settings -> BackPressHandler(true) {
            setCurrentViewState(CurrentViewState.Browse)
        }
        CurrentViewState.Browse -> if (currentParent == null) {
            BackPressHandler(false) {}
        } else {
            BackPressHandler(true) {
                showNode(currentParent.parent)
            }
        }
        CurrentViewState.Player -> BackPressHandler(true) {
            setCurrentViewState(CurrentViewState.Browse)
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
                                setFilteredItems(currentItems.filter {
                                    if (query.isEmpty())
                                        true
                                    else it.path.contains(query, ignoreCase = true)
                                } as MutableList<FileSystemNode>)
                            },
                            onClose = { setSearchOpened(false) }
                        )
                    } else {
                        TopAppBarSearchButton(onClick = { setSearchOpened(true) })
                    }
                }
            })
        {
            if (items.isEmpty()) {
                Text(text = "The folder is empty...")
            } else {
                LazyColumn(
                    state = lazyListState,
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
                ) {
                    if (isSearchOpened && searchQuery.isNotEmpty()) {
                        itemsIndexed(filteredCurrentItems) { _, item ->
                            FileSystemListElement(element = item, handleItem = ::handleItemClick)
                            Divider(color = Color.Black)
                        }
                    } else {
                        itemsIndexed(currentItems) { _, item ->
                            FileSystemListElement(element = item, handleItem = ::handleItemClick)
                            Divider(color = Color.Black)
                        }
                    }
                }
            }
        }
    }
}