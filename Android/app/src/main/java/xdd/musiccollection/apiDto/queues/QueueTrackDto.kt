package xdd.musiccollection.apiDto.queues

import xdd.musiccollection.apiDto.files.FileSystemNodeDto

data class QueueTrackDto(var position: Int, var track: FileSystemNodeDto)