package xdd.musiccollection.apiClient.retrofit

import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import xdd.musiccollection.apiDto.users.AuthCredentialsDto
import xdd.musiccollection.apiDto.users.UserDto

interface IUsersApiClient {
    @POST("users/register")
    suspend fun register(@Body authCredentialsDto: AuthCredentialsDto): UserDto

    @POST("users/login")
    suspend fun login(@Body authCredentialsDto: AuthCredentialsDto): UserDto
}