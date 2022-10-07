package xdd.musiccollection.apiClient

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

    suspend fun readRoot(id: UUID): Result<FileSystemRootDto> {
        return ApiClient.performRequest { client.readRoot(id) }
    }

    suspend fun readNode(rootId: UUID, nodeId: UUID): Result<FileSystemNodeDto> {
        return ApiClient.performRequest { client.readNode(rootId, nodeId) }
    }

    suspend fun readNodeAsDirectory(
        rootId: UUID,
        nodeId: UUID,
        skip: Int = 0,
        take: Int = 50
    ): Result<Array<FileSystemNodeDto>> {
        return ApiClient.performRequest { client.readNodeAsDirectory(rootId, nodeId, skip, take) }
    }

    suspend fun readAllFilesInNode(rootId: UUID, nodeId: UUID): Result<Array<UUID>> {
        return ApiClient.performRequest { client.readAllFilesInNode(rootId, nodeId) }
    }

    private val client: IFilesApiClient =
        RetrofitClient.getClient().create(IFilesApiClient::class.java)
}