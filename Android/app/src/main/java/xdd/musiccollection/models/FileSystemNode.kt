package xdd.musiccollection.models

import java.util.*

data class FileSystemNode(val id: UUID, val parent: FileSystemNode?, val path: String, val type: NodeType);