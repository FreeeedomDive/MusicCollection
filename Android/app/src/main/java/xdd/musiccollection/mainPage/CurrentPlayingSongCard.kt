package xdd.musiccollection.mainPage

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.Card
import androidx.compose.material.Icon
import androidx.compose.material.IconButton
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

import xdd.musiccollection.R
import xdd.musiccollection.extensions.shorten
import xdd.musiccollection.mainPage.viewModels.SongViewModel

@Composable
fun CurrentPlayingSongCard(songViewModel: SongViewModel) {
    Card(
        modifier = Modifier
            .padding(8.dp)
            .height(65.dp)
            .clip(RoundedCornerShape(8.dp))
            .fillMaxWidth()
    ) {
        Row(
            modifier = Modifier
                .padding(horizontal = 8.dp)
                .fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Row(
                modifier = Modifier.padding(vertical = 4.dp),
                verticalAlignment = Alignment.CenterVertically
            ) {
                Column(
                    horizontalAlignment = Alignment.Start,
                    modifier = Modifier.padding(all = 8.dp)
                ) {
                    if (songViewModel.currentSelectedTrack == null) {
                        Text(
                            text = "No selected track"
                        )
                    } else if (songViewModel.currentSelectedTrack!!.tags == null) {
                        Text(
                            text = songViewModel.currentSelectedTrack!!.fileName().shorten(25)
                        )
                    } else {
                        Column {
                            Text(
                                text = songViewModel.currentSelectedTrack!!.tags!!.trackName?.shorten(25) ?: ""
                            )
                            Text(
                                text = songViewModel.currentSelectedTrack!!.tags!!.artist?.shorten(25) ?: "",
                                fontSize = 12.sp,
                                color = Color.Gray
                            )
                        }
                    }
                }
            }
            Row(
                modifier = Modifier
                    .padding(vertical = 4.dp)
                    .fillMaxHeight(),
                horizontalArrangement = Arrangement.End,
                verticalAlignment = Alignment.CenterVertically
            ) {
                IconButton(onClick = {}) {
                    Icon(
                        painter = painterResource(id = R.drawable.previous),
                        contentDescription = "back button"
                    )
                }
                IconButton(onClick = {
                    if (songViewModel.isPlaying) {
                        songViewModel.stopPlaying()
                        // sampleSong.pause()
                    } else {
                        songViewModel.resumePlaying()
                        // sampleSong.start()
                    }
                }) {
                    Icon(
                        painter = painterResource(
                            id = if (songViewModel.isPlaying) R.drawable.pause else R.drawable.play
                        ),
                        contentDescription = "play/pause button"
                    )
                }
                IconButton(onClick = {}) {
                    Icon(
                        painter = painterResource(id = R.drawable.next),
                        contentDescription = "next button"
                    )
                }
            }
        }
    }
}