package xdd.musiccollection.auth

import androidx.compose.foundation.layout.Column
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import java.util.UUID

enum class AuthenticationState{
    Login,
    Register
}

@Composable
fun AuthenticationPage(showMainPage: (userId: UUID) -> Unit) {
    val (currentAuthenticationState, setAuthenticationState) = remember { mutableStateOf(AuthenticationState.Login) }

    Column {
        if (currentAuthenticationState == AuthenticationState.Login){
            LoginPage(showMainPage) { setAuthenticationState(AuthenticationState.Register) }
        }
        if (currentAuthenticationState == AuthenticationState.Register){
            RegistrationPage(showMainPage) { setAuthenticationState(AuthenticationState.Login) }
        }
    }
}