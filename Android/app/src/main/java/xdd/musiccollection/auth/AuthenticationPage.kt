package xdd.musiccollection.auth

import androidx.compose.foundation.layout.Column
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember

enum class AuthenticationState{
    Login,
    Register
}

@Composable
fun AuthenticationPage() {
    val (currentAuthenticationState, setAuthenticationState) = remember { mutableStateOf(AuthenticationState.Login) }

    Column {
        if (currentAuthenticationState == AuthenticationState.Login){
            LoginPage { setAuthenticationState(AuthenticationState.Register) }
        }
        if (currentAuthenticationState == AuthenticationState.Register){
            RegistrationPage { setAuthenticationState(AuthenticationState.Login) }
        }
    }
}