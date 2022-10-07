package xdd.musiccollection.apiDto.music

data class AudioFileTagsDto(
    var artist: String?,
    var album: String?,
    var trackName: String?,
    var duration: String?,
    var format: String?,
    var sampleFrequency: Int?,
    var bitRate: Int?,
    var bitDepth: Int?,
)
