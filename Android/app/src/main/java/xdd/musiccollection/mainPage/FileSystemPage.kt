package xdd.musiccollection.mainPage

import android.util.Log
import android.widget.Toast
import androidx.compose.foundation.gestures.scrollable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.LazyListState
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material.TabRowDefaults.Divider
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import xdd.musiccollection.helpers.Generator
import xdd.musiccollection.models.FileSystemNode
import xdd.musiccollection.models.NodeType

@Composable
fun FileSystemPage() {
    val items = Generator().generateDirectories(null);
    val currentContext = LocalContext.current;
    val (currentItems, setItems) = remember { mutableStateOf(items) }
    val lazyListState: LazyListState = rememberLazyListState()

    fun handleItemClick(element: FileSystemNode) {
        if (element.type == NodeType.Back) {
            setItems(Generator().generateDirectories(element.parent))
        }
        if (element.type == NodeType.File) {
            Toast
                .makeText(currentContext, "This is file ${element.path}", Toast.LENGTH_LONG)
                .show()
        }
        if (element.type == NodeType.Directory) {
            setItems(Generator().generateDirectories(element))
        }
    }

    if (items.isEmpty()) {
        Text(text = "Empty list")
    } else {
        LazyColumn(
            state = lazyListState,
            modifier = Modifier
                .fillMaxSize()
        ) {
            itemsIndexed(currentItems) { _, item ->
                FileSystemListElement(element = item, handleItem = ::handleItemClick)
                Divider(color = Color.Black)
            }
        }
    }
}