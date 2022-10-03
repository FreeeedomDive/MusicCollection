package xdd.musiccollection.apiClient

import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import xdd.musiccollection.apiDto.users.AuthCredentialsDto
import xdd.musiccollection.apiDto.users.UserDto

interface IUsersApiClient {
    @POST("users/register")
    suspend fun register(@Body authCredentialsDto: AuthCredentialsDto): UserDto

    @POST("users/find")
    suspend fun find(@Body authCredentialsDto: AuthCredentialsDto): UserDto
}