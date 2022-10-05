package xdd.musiccollection.apiDto

class Result<T>(val isSuccess: Boolean, val statusCode: Int, val value: T?) {
    companion object {
        fun <T> ok(value: T): Result<T> {
            return Result(true, 200, value)
        }

        fun <T> fail(statusCode: Int): Result<T> {
            return Result(false, statusCode, null);
        }
    }
}