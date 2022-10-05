package xdd.musiccollection.apiDto.files

import com.google.gson.annotations.SerializedName

enum class NodeTypeDto {
    @SerializedName("0")
    File,
    @SerializedName("1")
    Directory
}