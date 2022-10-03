package xdd.musiccollection.defaultComponents

import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.unit.dp

enum class RoundedTextFieldPosition {
    Top,
    Middle,
    Bottom
}

@Composable
fun RoundedTextField(
    value: String,
    onValueChange: (String) -> Unit,
    label: @Composable () -> Unit,
    backgroundColor: Color = Color.White,
    position: RoundedTextFieldPosition = RoundedTextFieldPosition.Middle,
    isPasswordInput: Boolean = false
) {
    val roundCornerValue = 16.dp
    TextField(
        value = value,
        onValueChange = onValueChange,
        
        label = label,
        colors = TextFieldDefaults.textFieldColors(
            backgroundColor = backgroundColor,
            unfocusedIndicatorColor = if (position == RoundedTextFieldPosition.Bottom)
                Color.Transparent
            else
                MaterialTheme.colors.onSurface.copy(alpha = TextFieldDefaults.UnfocusedIndicatorLineOpacity),
            focusedIndicatorColor = if (position == RoundedTextFieldPosition.Bottom)
                Color.Transparent
            else
                MaterialTheme.colors.primary.copy(alpha = ContentAlpha.high)
        ),
        visualTransformation = if (isPasswordInput)
            PasswordVisualTransformation()
        else
            VisualTransformation.None,
        keyboardOptions = if (isPasswordInput)
            KeyboardOptions(autoCorrect = false, keyboardType = KeyboardType.Password)
        else
            KeyboardOptions.Default,
        shape = when (position) {
            RoundedTextFieldPosition.Top -> RoundedCornerShape(roundCornerValue, roundCornerValue, 0.dp, 0.dp)
            RoundedTextFieldPosition.Middle -> RoundedCornerShape(0.dp, 0.dp, 0.dp, 0.dp)
            RoundedTextFieldPosition.Bottom -> RoundedCornerShape(0.dp, 0.dp, roundCornerValue, roundCornerValue)
        },
        modifier = Modifier.fillMaxWidth()
    )
}