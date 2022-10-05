package xdd.musiccollection.mainPage

import androidx.compose.foundation.Image
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.material.CircularProgressIndicator
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import xdd.musiccollection.R
import xdd.musiccollection.models.NodeModel
import xdd.musiccollection.models.NodeType

@Composable
fun FileSystemListElement(
    element: NodeModel,
    handleItem: (element: NodeModel, disableSelectedElementLoading: () -> Unit) -> Unit
) {
    val (isLoading, setLoading) = remember { mutableStateOf(false) }

    Row(
        verticalAlignment = Alignment.CenterVertically,
        modifier = Modifier
            .clickable {
                setLoading(true)
                handleItem(element) { setLoading(false) }
            }
            .fillMaxWidth()
    ) {
        val painter = when (element.type) {
            NodeType.File -> painterResource(R.drawable.music_fill)
            NodeType.Directory -> painterResource(R.drawable.folder)
            NodeType.Root -> painterResource(R.drawable.folder)
            NodeType.Back -> painterResource(R.drawable.folder_up)
        }
        Box(
            Modifier
                .width(50.dp)
                .height(50.dp)
        ) {
            if (isLoading) {
                CircularProgressIndicator(
                    color = Color.Black,
                    modifier = Modifier
                        .padding(7.dp)
                        .height(36.dp)
                        .width(36.dp)
                )
            } else {
                Image(
                    painter, "FileSystemListElementType",
                    Modifier
                        .height(50.dp)
                        .width(50.dp)
                )
            }
        }
        Text(
            text = when (element.type) {
                NodeType.Back -> ".."
                NodeType.Root -> element.path
                else -> element.path.split("\\").last()
            },
            fontSize = 20.sp,
            modifier = Modifier
                .padding(horizontal = 4.dp)
                .fillMaxSize()
        )
    }
}