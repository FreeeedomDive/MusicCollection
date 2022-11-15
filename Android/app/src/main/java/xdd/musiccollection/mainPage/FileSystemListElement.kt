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
import xdd.musiccollection.extensions.pluralize
import xdd.musiccollection.models.NodeModel
import xdd.musiccollection.models.NodeType

@Composable
fun FileSystemListElement(
    element: NodeModel,
    handleItem: (element: NodeModel, disableSelectedElementLoading: () -> Unit) -> Unit
) {
    val (isLoading, setLoading) = remember { mutableStateOf(false) }

    fun buildTagsString(): String {
        if (element.tags == null) {
            return ""
        }
        return arrayOf(
            element.tags.duration,
            element.tags.format,
            if (element.tags.bitRate != null && element.tags.bitRate != 0)
                "${element.tags.bitRate} kbps" else null,
            if (element.tags.sampleFrequency != null && element.tags.sampleFrequency != 0)
                "${element.tags.sampleFrequency} Hz" else null,
            if (element.tags.bitDepth != null && element.tags.bitDepth != 0)
                "${element.tags.bitDepth} bits" else null,
        ).filterNotNull().joinToString("  |  ")
    }

    fun buildDirectoryDataString(): String {
        val directories = element.directoryData!!.directories
        val files = element.directoryData.files
        return arrayOf(
            if (directories != 0) "$directories ${"folder".pluralize(directories)}" else null,
            if (files != 0) "$files ${"file".pluralize(files)}" else null
        ).filterNotNull().joinToString("  |  ").ifEmpty { "Empty folder" }
    }

    fun buildNameString(): String {
        return element.tags?.trackName ?: element.fileName()
    }

    fun buildArtistAlbumString(): String {
        if (element.tags == null){
            return ""
        }

        return "${element.tags.artist} - ${element.tags.album}"
    }

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
        when (element.type) {
            NodeType.Root -> Column{
                FileSystemListElementText(element.path, 20)
            }
            NodeType.Back -> Column {
                FileSystemListElementText("..", 20)
            }
            NodeType.Directory -> Column {
                FileSystemListElementText(element.path.split("\\").last(), 20)
                FileSystemListElementText(buildDirectoryDataString(), 14)
            }
            NodeType.File -> Column {
                FileSystemListElementText(buildNameString(), 20)
                FileSystemListElementText(buildArtistAlbumString(), 16)
                FileSystemListElementText(buildTagsString(), 14)
            }
        }
    }
}