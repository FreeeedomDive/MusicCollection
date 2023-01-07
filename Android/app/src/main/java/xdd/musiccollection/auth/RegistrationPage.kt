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
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import retrofit2.HttpException
import xdd.musiccollection.R
import xdd.musiccollection.apiClient.UsersApiClient
import xdd.musiccollection.apiClient.retrofit.IUsersApiClient
import xdd.musiccollection.apiClient.retrofit.RetrofitClient
import xdd.musiccollection.apiDto.users.AuthCredentialsDto
import xdd.musiccollection.defaultComponents.RoundedCornerOutlinedButton
import xdd.musiccollection.defaultComponents.RoundedTextField
import xdd.musiccollection.defaultComponents.RoundedTextFieldPosition
import xdd.musiccollection.ui.theme.*
import java.util.*

@Composable
fun RegistrationPage(showMainPage: (userId: UUID) -> Unit, switchToLogin: () -> Unit) {
    val (loginValue, setLoginValue) = remember { mutableStateOf("") }
    val (passwordValue, setPasswordValue) = remember { mutableStateOf("") }
    val (confirmPasswordValue, setConfirmPasswordValue) = remember { mutableStateOf("") }
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
                            PurpleColorPalette3,
                            PurpleColorPalette2,
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
                    position = RoundedTextFieldPosition.Middle,
                    value = passwordValue,
                    onValueChange = { setPasswordValue(it) },
                    label = { Text(text = "Password") },
                    isPasswordInput = true
                )
                RoundedTextField(
                    position = RoundedTextFieldPosition.Bottom,
                    value = confirmPasswordValue,
                    onValueChange = { setConfirmPasswordValue(it) },
                    label = { Text(text = "Confirm password") },
                    isPasswordInput = true
                )
                Text(
                    text = "Already have an account? Log in",
                    textAlign = TextAlign.Center,
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 8.dp)
                        .clickable {
                            switchToLogin()
                        }
                )
                RoundedCornerOutlinedButton(
                    onClick = {
                        if (passwordValue != confirmPasswordValue) {
                            Toast.makeText(
                                currentContext,
                                "Passwords are not the same",
                                Toast.LENGTH_LONG
                            ).show()
                            return@RoundedCornerOutlinedButton
                        }
                        composableScope.launch {
                            setLoading(true)
                            val registerResult = authClient.register(loginValue, passwordValue)
                            when(registerResult.statusCode){
                                200 -> {
                                    Log.i("Register", "Successful register as ${registerResult.value?.login}")
                                    showMainPage(registerResult.value!!.id!!)
                                }
                                409 -> {
                                    Log.e("Register", "")
                                    Toast.makeText(currentContext, "This login is unavailable", Toast.LENGTH_LONG).show()
                                }
                                else -> {
                                    Log.e("Register", "Unexpected error")
                                    Toast.makeText(currentContext, "Unexpected error", Toast.LENGTH_LONG).show()
                                }
                            }
                            Log.i("Register", "Register try finished")
                            setLoading(false)
                        }
                    },
                    modifier = Modifier
                        .height(60.dp)
                        .fillMaxWidth(),
                    isLoading = isLoading,
                    text = "Register",
                    backgroundColor = PurpleColorPalette1,
                    iconId = R.drawable.user_add_alt_fill
                )
            }
        }
    }
}