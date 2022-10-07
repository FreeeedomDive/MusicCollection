package xdd.musiccollection.models

import xdd.musiccollection.apiDto.files.DirectoryData
import xdd.musiccollection.apiDto.music.AudioFileTagsDto
import java.util.*

data class NodeModel(
    val id: UUID,
    val parent: NodeModel?,
    val path: String,
    val type: NodeType,
    val directoryData: DirectoryData?,
    val tags: AudioFileTagsDto?
)