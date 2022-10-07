package xdd.musiccollection.mainPage

import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

@Composable
fun FileSystemListElementText(text: String, textSize: Int) {
    Text(
        text = text,
        color = Color.Black,
        fontSize = textSize.sp,
        modifier = Modifier.fillMaxSize()
    )
}