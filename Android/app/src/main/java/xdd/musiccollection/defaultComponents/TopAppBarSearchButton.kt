package xdd.musiccollection.defaultComponents

import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material.Icon
import androidx.compose.material.IconButton
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Search
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp

@Composable
fun TopAppBarSearchButton(onClick: () -> Unit){
    IconButton(onClick = onClick) {
        Icon(
            Icons.Default.Search,
            contentDescription = "Search",
            modifier = Modifier
                .padding(15.dp)
                .size(24.dp)
        )
    }
}