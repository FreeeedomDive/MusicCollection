package xdd.musiccollection.apiClient

import okhttp3.ResponseBody
import xdd.musiccollection.apiClient.retrofit.IFilesApiClient
import xdd.musiccollection.apiClient.retrofit.RetrofitClient
import xdd.musiccollection.apiDto.Result
import xdd.musiccollection.apiDto.files.FileSystemNodeDto
import xdd.musiccollection.apiDto.files.FileSystemRootDto
import java.util.*

class FilesApiClient {
    suspend fun readAllRoots(): Result<Array<FileSystemRootDto>> {
        return ApiClient.performRequest { client.readAllRoots() }
    }

    suspend fun readNode(nodeId: UUID): Result<FileSystemNodeDto> {
        return ApiClient.performRequest { client.readNode(nodeId) }
    }

    suspend fun readNodeAsDirectory(
        nodeId: UUID,
        skip: Int = 0,
        take: Int = 50
    ): Result<Array<FileSystemNodeDto>> {
        return ApiClient.performRequest { client.readNodeAsDirectory(nodeId, skip, take) }
    }

    suspend fun download(nodeId: UUID): Result<ResponseBody?> {
        return ApiClient.performRequest { client.download(nodeId).body() }
    }

    private val client: IFilesApiClient = RetrofitClient.getClient().create(IFilesApiClient::class.java)
}