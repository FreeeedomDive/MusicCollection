package xdd.musiccollection.mainPage

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import kotlinx.coroutines.launch

import xdd.musiccollection.R
import xdd.musiccollection.extensions.shorten
import xdd.musiccollection.mainPage.viewModels.FileSystemPageViewModel
import xdd.musiccollection.mainPage.viewModels.PlayingState

@Composable
fun CurrentPlayingSongCard(viewModel: FileSystemPageViewModel) {
    val composableScope = rememberCoroutineScope()

    Card(
        backgroundColor = Color.Black,
        modifier = Modifier
            .padding(8.dp)
            .height(64.dp)
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
                    val topText: String
                    val bottomText: String
                    if (viewModel.currentSelectedTrack == null) {
                        topText = "No selected track"
                        bottomText = "0 tracks in queue"
                    } else if (viewModel.currentSelectedTrack!!.tags == null) {
                        topText = viewModel.currentSelectedTrack!!.fileName().shorten(25)
                        bottomText = "No tags available"
                    } else {
                        topText = viewModel.currentSelectedTrack!!.tags!!.trackName?.shorten(25) ?: ""
                        bottomText = viewModel.currentSelectedTrack!!.tags!!.artist?.shorten(25) ?: ""
                    }
                    Column {
                        Text(
                            text = topText
                        )
                        Text(
                            text = bottomText,
                            fontSize = 12.sp,
                            color = Color.LightGray
                        )
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
                IconButton(onClick = {
                    composableScope.launch {
                        viewModel.movePrevious()
                    }
                }) {
                    Icon(
                        painter = painterResource(id = R.drawable.previous),
                        contentDescription = "back button"
                    )
                }
                if (viewModel.playingState == PlayingState.Loading) {
                    CircularProgressIndicator(
                        color = Color.White,
                        modifier = Modifier
                            .height(30.dp)
                            .width(30.dp)
                    )
                } else {
                    IconButton(onClick = {
                        when (viewModel.playingState) {
                            PlayingState.Loading -> {
                                return@IconButton
                            }
                            PlayingState.Play -> {
                                viewModel.stopPlaying()
                            }
                            else -> {
                                viewModel.resumePlaying()
                            }
                        }
                    }) {
                        Icon(
                            painter = painterResource(
                                id = if (viewModel.playingState == PlayingState.Pause)
                                    R.drawable.play
                                else R.drawable.pause
                            ),
                            contentDescription = "play/pause button"
                        )
                    }
                }

                IconButton(onClick = {
                    composableScope.launch {
                        viewModel.moveNext()
                    }
                }) {
                    Icon(
                        painter = painterResource(id = R.drawable.next),
                        contentDescription = "next button"
                    )
                }
            }
        }
    }
}