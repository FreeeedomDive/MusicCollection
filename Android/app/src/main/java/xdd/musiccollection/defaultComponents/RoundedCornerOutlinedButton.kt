package xdd.musiccollection.defaultComponents

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp

@Composable
fun RoundedCornerOutlinedButton(
    onClick: () -> Unit,
    isLoading: Boolean,
    text: String,
    iconId: Int?,
    backgroundColor: Color,
    modifier: Modifier,
) {
    OutlinedButton(
        onClick = onClick,
        modifier = modifier,
        shape = RoundedCornerShape(50),
        colors = ButtonDefaults.buttonColors(
            backgroundColor = backgroundColor
        )
    )
    {
        if (isLoading) {
            CircularProgressIndicator(color = Color.White)
        }
        else{
            if (iconId != null) {
                Image(painter = painterResource(id = iconId), contentDescription = "Icon")
            }
            Text(
                text = text,
                modifier = Modifier.padding(8.dp)
            )
        }
    }
}