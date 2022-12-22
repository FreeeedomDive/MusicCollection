package xdd.musiccollection

import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.platform.LocalContext
import xdd.musiccollection.auth.AuthenticationPage
import xdd.musiccollection.mainPage.FileSystemPage
import xdd.musiccollection.mainPage.viewModels.FileSystemPageViewModel

enum class CurrentState {
    Authentication,
    MainPage,
}

@Composable
fun App() {
    val (currentState, setCurrentState) = remember { mutableStateOf(CurrentState.Authentication) }

    if (currentState == CurrentState.Authentication) {
        AuthenticationPage { setCurrentState(CurrentState.MainPage) }
    }
    if (currentState == CurrentState.MainPage){
        FileSystemPage(FileSystemPageViewModel())
    }
}