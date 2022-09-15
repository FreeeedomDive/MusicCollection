package xdd.musiccollection.auth

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.material.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import xdd.musiccollection.defaultComponents.RoundedCornerOutlinedButton
import xdd.musiccollection.ui.theme.BlueGradientBottom
import xdd.musiccollection.ui.theme.BlueGradientTop

@Composable
fun LoginPage(switchToRegistration: () -> Unit) {
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
                            BlueGradientTop,
                            BlueGradientBottom,
                        )
                    )
                ),
            verticalArrangement = Arrangement.Center
        ) {
            Column(
                Modifier.padding(16.dp)
            ) {
                OutlinedTextField(
                    value = loginValue,
                    onValueChange = { setLoginValue(it) },
                    label = { Text(text = "Login") },
                    modifier = Modifier.fillMaxWidth()
                )
                OutlinedTextField(
                    value = passwordValue,
                    onValueChange = { setPasswordValue(it) },
                    label = { Text(text = "Password") },
                    visualTransformation = PasswordVisualTransformation(),
                    modifier = Modifier.fillMaxWidth()
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
                    onClick = { setLoading(!isLoading) },
                    modifier = Modifier
                        .height(60.dp)
                        .fillMaxWidth(),
                    isLoading = isLoading,
                    text = "Submit"
                )
            }
        }
    }
}