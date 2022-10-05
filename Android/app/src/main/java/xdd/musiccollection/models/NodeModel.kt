package xdd.musiccollection.models

import xdd.musiccollection.apiDto.music.AudioFileTagsDto
import java.util.*

data class NodeModel(val id: UUID, val parent: NodeModel?, val path: String, val type: NodeType, val tags: AudioFileTagsDto?) : Comparable<NodeModel>{

    override fun compareTo(other: NodeModel): Int {
        val typeCompare = type.compareTo(other.type)
        if (typeCompare != 0) {
            return typeCompare
        }
        return path.compareTo(other.path)
    }
}