package xdd.musiccollection.mainPage

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import xdd.musiccollection.R
import xdd.musiccollection.models.FileSystemNode
import xdd.musiccollection.models.NodeType

@Composable
fun FileSystemListElement(element: FileSystemNode, handleItem: (element: FileSystemNode) -> Unit) {
    Row(
        verticalAlignment = Alignment.CenterVertically,
        modifier = Modifier
            .clickable { handleItem(element) }
            .fillMaxWidth()
    ) {
        val painter = when (element.type) {
            NodeType.File -> painterResource(R.drawable.music_fill)
            NodeType.Directory -> painterResource(R.drawable.folder)
            NodeType.Root -> painterResource(R.drawable.folder)
            NodeType.Back -> painterResource(R.drawable.folder_up)
        }
        Image(
            painter, "FileSystemListElementType",
            Modifier.height(50.dp).width(50.dp))
        Text(
            text = if (element.type == NodeType.Back) ".." else element.path,
            fontSize = 20.sp,
            modifier = Modifier.padding(horizontal = 4.dp).fillMaxSize()
        )
    }
}