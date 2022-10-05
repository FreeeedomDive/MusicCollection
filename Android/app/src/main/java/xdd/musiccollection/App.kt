package xdd.musiccollection

import android.util.Log
import android.widget.Toast
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.platform.LocalContext
import kotlinx.coroutines.launch
import xdd.musiccollection.apiClient.FilesApiClient
import xdd.musiccollection.apiDto.files.convertToModel
import xdd.musiccollection.auth.AuthenticationPage
import xdd.musiccollection.mainPage.FileSystemPage
import xdd.musiccollection.models.NodeModel

enum class CurrentState {
    Authentication,
    MainPage,
}

@Composable
fun App() {
    val (currentState, setCurrentState) = remember { mutableStateOf(CurrentState.Authentication) }
    // костыль??
    val composableScope = rememberCoroutineScope()
    val currentContext = LocalContext.current
    val filesApiClient = FilesApiClient()

    fun showErrorToast(message: String? = null) {
        Toast.makeText(currentContext, message ?: "Can't load this page now!", Toast.LENGTH_LONG)
            .show()
    }

    if (currentState == CurrentState.Authentication) {
        AuthenticationPage { setCurrentState(CurrentState.MainPage) }
    }
    if (currentState == CurrentState.MainPage){
        FileSystemPage()
    }
}