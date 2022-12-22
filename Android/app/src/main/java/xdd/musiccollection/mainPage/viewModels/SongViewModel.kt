package xdd.musiccollection.mainPage.viewModels

import android.media.AudioAttributes
import android.media.MediaPlayer
import android.os.Environment
import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.ResponseBody
import xdd.musiccollection.apiClient.FilesApiClient
import xdd.musiccollection.models.NodeModel
import java.io.*
import java.util.*

enum class PlayingState {
    Play,
    Pause,
    Loading
}

class SongViewModel : ViewModel() {
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

    var playingState by mutableStateOf(PlayingState.Pause)
        private set

    fun resumePlaying() {
        playingState = PlayingState.Play
        mediaPlayer.start()
    }

    fun stopPlaying() {
        playingState = PlayingState.Pause
        if (mediaPlayer.isPlaying) {
            mediaPlayer.pause()
        }
    }

    private fun resetPlaying() {
        stopPlaying()
        mediaPlayer.reset()
    }

    suspend fun setCurrentTrack(rootId: UUID, track: NodeModel) {
        currentSelectedTrack = track
        resetPlaying()
        playingState = PlayingState.Loading
        val coroutineScope = CoroutineScope(Dispatchers.IO)
        Log.i("Coroutine", "Before")
        coroutineScope.launch {
            Log.i("Coroutine", "Start")
            val response = FilesApiClient().download(rootId, track.id)
            if (!response.isSuccess) {
                return@launch
            }
            val fileStream = saveFile(response.value, track) ?: return@launch
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
            Log.i("Coroutine", "End")
        }
        Log.i("Coroutine", "After")
    }

    private fun saveFile(body: ResponseBody?, track: NodeModel): FileInputStream? {
        if (body == null)
            return null
        var input: InputStream? = null
        try {
            val downloadsDirectory = getDownloadsDirectory()
            prepareDownloadsDirectory(downloadsDirectory)
            input = body.byteStream()
            val file = File(downloadsDirectory, buildFileName(track))
            val fos = FileOutputStream(file)
            fos.use { out ->
                input.copyTo(out)
            }
            return FileInputStream(file)
        } catch (e: Exception) {
            Log.e("saveFile exception", e.toString())
        } finally {
            input?.close()
        }
        return null
    }

    private fun getDownloadsDirectory(): File {
        val path = Environment.DIRECTORY_DOWNLOADS + "/MusicCollectionCache"
        val downloadsDirectory = Environment.getExternalStoragePublicDirectory(path)
        if (!downloadsDirectory.exists()) {
            downloadsDirectory.mkdir()
        }

        return downloadsDirectory
    }

    private fun prepareDownloadsDirectory(directory: File) {
        directory.listFiles()?.forEach { it.delete() }
    }

    private fun buildFileName(track: NodeModel): String {
        if (track.tags!!.artist != null && track.tags.trackName != null) {
            return "${track.tags.artist} - ${track.tags.trackName}.${track.tags.format}"
        }

        if (track.tags.trackName != null) {
            return "${track.tags.trackName}.${track.tags.format}"
        }

        return "${track.id}.${track.tags.format}"
    }
}