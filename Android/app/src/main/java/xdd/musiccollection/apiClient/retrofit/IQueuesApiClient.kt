package xdd.musiccollection.apiClient.retrofit

import okhttp3.Response
import okhttp3.ResponseBody
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import xdd.musiccollection.apiDto.files.FileSystemNodeDto
import xdd.musiccollection.apiDto.queues.QueueTrackDto
import java.util.*

interface IQueuesApiClient {
    @POST("queues/{userId}/create/{contextId}")
    suspend fun createQueue(
        @Path("userId") userId: UUID,
        @Path("contextId") contextId: UUID
    ): ResponseBody

    @GET("queues/{userId}/context")
    suspend fun getCurrentQueueContext(
        @Path("userId") userId: UUID
    ): FileSystemNodeDto

    @DELETE("queues/{userId}")
    suspend fun clearQueue(
        @Path("userId") userId: UUID
    ): ResponseBody

    @GET("queues/{userId}/list")
    suspend fun getQueue(
        @Path("userId") userId: UUID
    ): Array<QueueTrackDto>

    @GET("queues/{userId}/current")
    suspend fun getCurrentTrack(
        @Path("userId") userId: UUID
    ): QueueTrackDto?

    @POST("queues/{userId}/move/previous")
    suspend fun movePrevious(
        @Path("userId") userId: UUID
    ): QueueTrackDto

    @POST("queues/{userId}/move/next")
    suspend fun moveNext(
        @Path("userId") userId: UUID
    ): QueueTrackDto

    @POST("queues/{userId}/move/{position}")
    suspend fun moveToPosition(
        @Path("userId") userId: UUID,
        @Path("position") position: Int
    ): QueueTrackDto

    @POST("queues/{userId}/shuffle/{shuffle}")
    suspend fun updateShuffle(
        @Path("userId") userId: UUID,
        @Path("shuffle") shuffle: Boolean
    ): ResponseBody
}