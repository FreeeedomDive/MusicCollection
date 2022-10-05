package xdd.musiccollection.apiClient

import retrofit2.HttpException
import xdd.musiccollection.apiDto.Result

class ApiClient<T> {
    companion object {
        suspend fun <T> performRequest(apiCall: suspend () -> T): Result<T> {
            return try {
                Result.ok(apiCall())
            } catch (e: HttpException) {
                Result.fail(e.code())
            } catch (e: Exception) {
                Result.fail(500)
            }
        }
    }
}