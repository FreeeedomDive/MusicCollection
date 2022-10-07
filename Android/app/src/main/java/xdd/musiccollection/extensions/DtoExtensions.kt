package xdd.musiccollection.extensions

import xdd.musiccollection.apiDto.files.FileSystemNodeDto
import xdd.musiccollection.apiDto.files.FileSystemRootDto
import xdd.musiccollection.apiDto.files.NodeTypeDto
import xdd.musiccollection.models.NodeModel
import xdd.musiccollection.models.NodeType

fun FileSystemNodeDto.convertToModel(parent: NodeModel? = null) : NodeModel{
    return NodeModel(this.id, parent, this.path, if (this.type == NodeTypeDto.File) NodeType.File else NodeType.Directory, this.directoryData, this.tags)
}

fun FileSystemRootDto.convertToModel() : NodeModel{
    return NodeModel(this.id, null, this.path, NodeType.Root, null, null)
}