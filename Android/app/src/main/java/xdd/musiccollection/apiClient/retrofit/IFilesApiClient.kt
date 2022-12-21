package xdd.musiccollection.apiClient.retrofit

import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query
import retrofit2.http.Streaming
import xdd.musiccollection.apiDto.files.FileSystemNodeDto
import xdd.musiccollection.apiDto.files.FileSystemRootDto
import java.util.*

interface IFilesApiClient {
    @GET("roots")
    suspend fun readAllRoots(): Array<FileSystemRootDto>

    @GET("roots/{rootId}/nodes/{nodeId}")
    suspend fun readNode(
        @Path("rootId") rootId: UUID,
        @Path("nodeId") nodeId: UUID
    ): FileSystemNodeDto

    @GET("roots/{rootId}/nodes/{nodeId}/ReadChildren")
    suspend fun readNodeAsDirectory(
        @Path("rootId") rootId: UUID,
        @Path("nodeId") nodeId: UUID,
        @Query("skip") skip: Int = 0,
        @Query("take") take: Int = 50
    ): Array<FileSystemNodeDto>

    @Streaming
    @GET("roots/{rootId}/nodes/{nodeId}/DownloadStream")
    suspend fun download(
        @Path("rootId") rootId: UUID,
        @Path("nodeId") nodeId: UUID
    ): Response<ResponseBody>
}