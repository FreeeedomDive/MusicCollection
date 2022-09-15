package xdd.musiccollection.helpers

import android.util.Log
import xdd.musiccollection.models.FileSystemNode
import xdd.musiccollection.models.NodeType
import java.util.*

class Generator {
    fun generateDirectories(parent: FileSystemNode?): MutableList<FileSystemNode> {
        val result = mutableListOf<FileSystemNode>()
        val itemsCount = Random().nextInt(20)
        if (parent != null) {
            result.add(FileSystemNode(
                parent.id,
                parent.parent,
                parent.path,
                NodeType.Back
            ))
        }
        val parentPath = parent?.path ?: ""
        for (i in 0..itemsCount){
            val path = generateText(Random().nextInt(5));
            val element = FileSystemNode(
                UUID.randomUUID(),
                parent,
                "${parentPath}/${path}",
                if (Random().nextInt(2) % 2 == 0) NodeType.File else NodeType.Directory
            )
            result.add(element);
        }
        result.sortedBy { item -> item.path }

        Log.i("Generator", "Generated ${result.count()} items")
        return result
    }

    private fun generateText(length: Int): String{
        val alphabet = "abcdefghijklmnopqrstuvwxyz";
        var name = ""
        for (i in 0..length) {
            name += alphabet[Random().nextInt(alphabet.length)]
        }

        return name
    }
}