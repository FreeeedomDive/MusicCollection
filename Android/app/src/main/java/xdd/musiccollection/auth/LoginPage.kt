package xdd.musiccollection.auth

import android.util.Log
import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Alignment.Companion.CenterHorizontally
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import retrofit2.Call
import retrofit2.Callback
import retrofit2.HttpException
import retrofit2.Response
import xdd.musiccollection.R
import xdd.musiccollection.apiClient.IUsersApiClient
import xdd.musiccollection.apiClient.RetrofitClient
import xdd.musiccollection.apiDto.users.AuthCredentialsDto
import xdd.musiccollection.apiDto.users.UserDto
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
    val authClient = RetrofitClient.getClient().create(IUsersApiClient::class.java)

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
                        composableScope.launch {
                            setLoading(true)
                            try {
                                val user = authClient.find(AuthCredentialsDto(loginValue, passwordValue))
                                Log.i(
                                    "Login",
                                    "Authenticated as user ${user.id} (${user.login})"
                                )
                                showMainPage()
                            } catch (e: HttpException) {
                                Toast
                                    .makeText(
                                        currentContext,
                                        "Login unsuccessful with code ${e.code()}",
                                        Toast.LENGTH_LONG
                                    )
                                    .show()
                                Log.e("Login", "Login unsuccessful with code ${e.code()}")
                            } finally {
                                setLoading(false)
                            }
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