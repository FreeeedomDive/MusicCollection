package xdd.musiccollection.models

import xdd.musiccollection.apiDto.files.DirectoryDataDto
import xdd.musiccollection.apiDto.music.AudioFileTagsDto
import java.util.*

data class NodeModel(
    val id: UUID,
    val parent: NodeModel?,
    val rootName: String?,
    val path: String,
    val type: NodeType,
    val directoryData: DirectoryDataDto?,
    val tags: AudioFileTagsDto?
)
{
    fun fileName(): String {
        return path.split("\\", "/").last();
    }
}