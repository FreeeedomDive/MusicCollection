package xdd.musiccollection.mainPage

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import xdd.musiccollection.models.FileSystemNode
import xdd.musiccollection.models.NodeType

@Composable
fun FileSystemListElement(element: FileSystemNode, handleItem: (element: FileSystemNode) -> Unit) {
    val color = when (element.type) {
        NodeType.File -> Color(0xff4287f5)
        NodeType.Directory -> Color(0xff0062ff)
        NodeType.Back -> Color(0xffb8bbff)
        NodeType.Root -> Color(0xff000457)
    }

    Column(
        Modifier
            .clickable { handleItem(element) }
            .fillMaxWidth()
            .background(color = color)
    ) {
        Text(text = element.type.toString(), modifier = Modifier.padding(horizontal = 8.dp))
        Text(
            text = if (element.type == NodeType.Back) ".." else element.path,
            modifier = Modifier.padding(horizontal = 8.dp)
        )
    }
}