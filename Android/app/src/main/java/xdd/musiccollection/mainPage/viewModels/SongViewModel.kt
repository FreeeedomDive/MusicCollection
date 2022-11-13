package xdd.musiccollection.mainPage.viewModels

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import xdd.musiccollection.models.NodeModel

class SongViewModel : ViewModel() {
    var currentSelectedTrack by mutableStateOf<NodeModel?>(null)
        private set

    var isPlaying by mutableStateOf(false)
        private set

    fun resumePlaying() {
        isPlaying = true
    }

    fun stopPlaying() {
        isPlaying = false
    }

    fun setCurrentTrack(track: NodeModel?) {
        currentSelectedTrack = track
    }
}