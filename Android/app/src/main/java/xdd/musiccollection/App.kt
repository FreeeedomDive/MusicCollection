package xdd.musiccollection

import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import xdd.musiccollection.auth.AuthenticationPage
import xdd.musiccollection.mainPage.FileSystemPage

enum class CurrentState {
    Authentication,
    Browse,
    Player
}

@Composable
fun App() {
    val (currentState, setCurrentState) = remember { mutableStateOf(CurrentState.Authentication) }


    if (currentState == CurrentState.Authentication) {
        AuthenticationPage { setCurrentState(CurrentState.Browse) }
    }
    if (currentState == CurrentState.Browse){
        FileSystemPage()
    }
}