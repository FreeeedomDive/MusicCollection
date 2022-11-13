package xdd.musiccollection.apiClient

import android.util.Log
import retrofit2.HttpException
import xdd.musiccollection.apiDto.Result

class ApiClient<T> {
    companion object {
        suspend fun <T> performRequest(apiCall: suspend () -> T): Result<T> {
            return try {
                Result.ok(apiCall())
            } catch (e: HttpException) {
                Log.e("Api Client", e.message())
                Result.fail(e.code())
            } catch (e: Exception) {
                Log.e("Api Client", e.message ?: "Unhandled exception without message")
                Result.fail(500)
            }
        }
    }
}