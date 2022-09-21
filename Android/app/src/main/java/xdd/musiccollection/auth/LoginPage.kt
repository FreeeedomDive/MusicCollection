package xdd.musiccollection.auth

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Alignment.Companion.CenterHorizontally
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import xdd.musiccollection.R
import xdd.musiccollection.defaultComponents.RoundedCornerOutlinedButton
import xdd.musiccollection.defaultComponents.RoundedTextField
import xdd.musiccollection.defaultComponents.RoundedTextFieldPosition
import xdd.musiccollection.ui.theme.*

@Composable
fun LoginPage(showMainPage: () -> Unit, switchToRegistration: () -> Unit) {
    val (loginValue, setLoginValue) = remember { mutableStateOf("") }
    val (passwordValue, setPasswordValue) = remember { mutableStateOf("") }
    val (isLoading, setLoading) = remember { mutableStateOf(false) }

    Surface {
        Column(
            Modifier
                .fillMaxSize()
                .background(
                    brush = Brush.verticalGradient(
                        colors = listOf(
                            BlueColorPalette3,
                            BlueColorPalette2,
                        )
                    )
                ),
            verticalArrangement = Arrangement.Center
        ) {
            Column(
                Modifier.padding(16.dp)
            ) {
                RoundedTextField(
                    position = RoundedTextFieldPosition.Top,
                    value = loginValue,
                    onValueChange = { setLoginValue(it) },
                    label = { Text(text = "Login") },
                )
                RoundedTextField(
                    position = RoundedTextFieldPosition.Bottom,
                    value = passwordValue,
                    onValueChange = { setPasswordValue(it) },
                    label = { Text(text = "Password") },
                    isPasswordInput = true
                )
                Text(
                    text = "Don't have an account? Register",
                    textAlign = TextAlign.Center,
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 8.dp)
                        .clickable {
                            switchToRegistration()
                        }
                )
                RoundedCornerOutlinedButton(
                    onClick = {
                        if (isLoading){
                            showMainPage()
                        }
                        setLoading(true)
                    },
                    modifier = Modifier
                        .height(60.dp)
                        .fillMaxWidth(),
                    isLoading = isLoading,
                    text = "Login",
                    backgroundColor = BlueColorPalette1,
                    iconId = R.drawable.sign_in_square,
                )
            }
        }
    }
}