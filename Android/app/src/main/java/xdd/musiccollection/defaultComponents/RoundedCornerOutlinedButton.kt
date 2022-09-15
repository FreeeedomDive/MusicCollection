package xdd.musiccollection.defaultComponents

import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.ButtonDefaults
import androidx.compose.material.CircularProgressIndicator
import androidx.compose.material.OutlinedButton
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp

@Composable
fun RoundedCornerOutlinedButton(
    onClick: () -> Unit,
    modifier: Modifier,
    isLoading: Boolean,
    text: String
) {
    OutlinedButton(
        onClick = onClick,
        modifier = modifier,
        shape = RoundedCornerShape(50),
        colors = ButtonDefaults.buttonColors(
            backgroundColor = Color.Transparent
        )
    )
    {
        if (isLoading) {
            CircularProgressIndicator()
        }
        else{
            Text(
                text = text,
                modifier = Modifier.padding(8.dp)
            )
        }
    }
}