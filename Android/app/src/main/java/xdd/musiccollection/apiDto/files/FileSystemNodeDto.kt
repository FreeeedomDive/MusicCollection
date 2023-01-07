package xdd.musiccollection.apiDto.files

import xdd.musiccollection.apiDto.music.AudioFileTagsDto
import java.util.*

data class FileSystemNodeDto(
    var id: UUID,
    var parentId: UUID?,
    var type: NodeTypeDto,
    var path: String,
    var directoryData: DirectoryDataDto?,
    var tags: AudioFileTagsDto?
)
