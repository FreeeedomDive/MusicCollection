package xdd.musiccollection.models

import java.util.*

data class FileSystemNode(val id: UUID, val parent: FileSystemNode?, val path: String, val type: NodeType) : Comparable<FileSystemNode>{

    override fun compareTo(other: FileSystemNode): Int {
        val typeCompare = type.compareTo(other.type)
        if (typeCompare != 0) {
            return typeCompare
        }
        return path.compareTo(other.path)
    }
}