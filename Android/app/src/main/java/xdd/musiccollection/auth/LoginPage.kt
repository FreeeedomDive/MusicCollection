package xdd.musiccollection.auth

import android.util.Log
import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.material.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import retrofit2.HttpException
import xdd.musiccollection.R
import xdd.musiccollection.apiClient.UsersApiClient
import xdd.musiccollection.apiDto.users.AuthCredentialsDto
import xdd.musiccollection.defaultComponents.RoundedCornerOutlinedButton
import xdd.musiccollection.defaultComponents.RoundedTextField
import xdd.musiccollection.defaultComponents.RoundedTextFieldPosition
import xdd.musiccollection.ui.theme.*
import java.lang.Exception

@Composable
fun LoginPage(showMainPage: () -> Unit, switchToRegistration: () -> Unit) {
    val (loginValue, setLoginValue) = remember { mutableStateOf("") }
    val (passwordValue, setPasswordValue) = remember { mutableStateOf("") }
    val (isLoading, setLoading) = remember { mutableStateOf(false) }
    val currentContext = LocalContext.current
    val composableScope = rememberCoroutineScope()
    val authClient = UsersApiClient()

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
                    color = Color.Black,
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
                        composableScope.launch {
                            setLoading(true)
                            val loginResult = authClient.auth(loginValue, passwordValue)
                            when(loginResult.statusCode){
                                200 -> {
                                    Log.i("Login", "Successful login as ${loginResult.value?.login}")
                                    showMainPage()
                                }
                                404 -> {
                                    Log.e("Login", "Invalid login or password")
                                    Toast.makeText(currentContext, "Invalid login or password", Toast.LENGTH_LONG).show()
                                }
                                else -> {
                                    Log.e("Login", "Unexpected error")
                                    Toast.makeText(currentContext, "Unexpected error", Toast.LENGTH_LONG).show()
                                }
                            }
                            Log.i("Login", "Login try finished")
                            setLoading(false)
                        }
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