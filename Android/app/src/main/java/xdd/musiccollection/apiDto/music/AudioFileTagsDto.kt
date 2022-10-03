package xdd.musiccollection.apiDto.music

data class AudioFileTagsDto(
    var artist: String?,
    var album: String?,
    var trackName: String?,
    var duration: String?,
    var format: String?,
    var sampleFrequency: String?,
    var bitRate: String?,
    var bitDepth: String?,
)
