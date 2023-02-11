package xdd.musiccollection.apiClient

import okhttp3.Response
import okhttp3.ResponseBody
import xdd.musiccollection.apiClient.retrofit.IQueuesApiClient
import xdd.musiccollection.apiClient.retrofit.RetrofitClient
import xdd.musiccollection.apiDto.Result
import xdd.musiccollection.apiDto.files.FileSystemNodeDto
import xdd.musiccollection.apiDto.queues.QueueTrackDto
import java.util.*

class QueuesApiClient {
    suspend fun createQueue(userId: UUID, contextId: UUID): Result<ResponseBody> {
        return ApiClient.performRequest { client.createQueue(userId, contextId) }
    }

    suspend fun getCurrentQueueContext(userId: UUID): Result<FileSystemNodeDto> {
        return ApiClient.performRequest { client.getCurrentQueueContext(userId) }
    }

    suspend fun clearQueue(userId: UUID): Result<ResponseBody> {
        return ApiClient.performRequest { client.clearQueue(userId) }
    }

    suspend fun getQueue(userId: UUID): Result<Array<QueueTrackDto>> {
        return ApiClient.performRequest { client.getQueue(userId) }
    }

    suspend fun getCurrentTrack(userId: UUID): Result<QueueTrackDto?> {
        return ApiClient.performRequest { client.getCurrentTrack(userId) }
    }

    suspend fun movePrevious(userId: UUID): Result<QueueTrackDto> {
        return ApiClient.performRequest { client.movePrevious(userId) }
    }

    suspend fun moveNext(userId: UUID): Result<QueueTrackDto> {
        return ApiClient.performRequest { client.moveNext(userId) }
    }

    suspend fun moveToPosition(userId: UUID, position: Int): Result<QueueTrackDto> {
        return ApiClient.performRequest { client.moveToPosition(userId, position) }
    }

    suspend fun updateShuffle(userId: UUID, shuffle: Boolean): Result<ResponseBody> {
        return ApiClient.performRequest { client.updateShuffle(userId, shuffle) }
    }

    private val client: IQueuesApiClient = RetrofitClient.getClient().create(IQueuesApiClient::class.java)
}