package xdd.musiccollection.mainPage.viewModels

import android.media.AudioAttributes
import android.media.MediaPlayer
import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import okhttp3.ResponseBody
import xdd.musiccollection.apiClient.FilesApiClient
import xdd.musiccollection.models.NodeModel
import java.io.*
import java.util.*

class SongViewModel(private val cacheDir: File) : ViewModel() {
    var currentSelectedTrack by mutableStateOf<NodeModel?>(null)
        private set
    private var mediaPlayer = MediaPlayer().apply {
        setAudioAttributes(
            AudioAttributes.Builder()
                .setContentType(AudioAttributes.CONTENT_TYPE_MUSIC)
                .setUsage(AudioAttributes.USAGE_MEDIA)
                .build()
        )
        setOnPreparedListener {
            resumePlaying()
        }
    }

    var isPlaying by mutableStateOf(false)
        private set

    fun resumePlaying() {
        isPlaying = true
        mediaPlayer.start()
    }

    fun stopPlaying() {
        isPlaying = false
        if (mediaPlayer.isPlaying) {
            mediaPlayer.pause()
        }
    }

    private fun resetPlaying() {
        stopPlaying()
        mediaPlayer.reset()
    }

    suspend fun setCurrentTrack(rootId: UUID, track: NodeModel) {
        val response = FilesApiClient().download(rootId, track.id)
        if (!response.isSuccess) {
            return
        }
        val fileStream = saveFile(response.value, track) ?: return
        currentSelectedTrack = track
        mediaPlayer.apply {
            resetPlaying()
            setDataSource(fileStream.fd)
            try {
                prepareAsync()
            } catch (e: IllegalStateException) {
                e.printStackTrace()
                reset()
            }
        }
    }

    private fun saveFile(body: ResponseBody?, track: NodeModel): FileInputStream? {
        if (body == null)
            return null
        var input: InputStream? = null
        try {
            input = body.byteStream()
            val file = File(cacheDir, "${track.id}.${track.tags!!.format}")
            val fos = FileOutputStream(file)
            fos.use { output ->
                val buffer = ByteArray(4 * 1024)
                var read: Int
                while (input.read(buffer).also { read = it } != -1) {
                    output.write(buffer, 0, read)
                }
                output.flush()
            }
            return FileInputStream(file)
        } catch (e: Exception) {
            Log.e("saveFile exception", e.toString())
        } finally {
            input?.close()
        }
        return null
    }
}