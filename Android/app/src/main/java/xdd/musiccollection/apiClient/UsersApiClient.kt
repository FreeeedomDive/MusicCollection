package xdd.musiccollection.apiClient

import xdd.musiccollection.apiClient.retrofit.IUsersApiClient
import xdd.musiccollection.apiClient.retrofit.RetrofitClient
import xdd.musiccollection.apiDto.Result
import xdd.musiccollection.apiDto.users.AuthCredentialsDto
import xdd.musiccollection.apiDto.users.UserDto

class UsersApiClient {
    suspend fun register(login: String, password: String): Result<UserDto> {
        return ApiClient.performRequest { client.register(AuthCredentialsDto(login, password)) }
    }

    suspend fun auth(login: String, password: String): Result<UserDto> {
        return ApiClient.performRequest { client.login(AuthCredentialsDto(login, password)) }
    }

    private val client: IUsersApiClient = RetrofitClient.getClient().create(IUsersApiClient::class.java)
}
