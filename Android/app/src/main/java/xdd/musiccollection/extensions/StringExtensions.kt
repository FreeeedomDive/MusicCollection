package xdd.musiccollection.extensions

fun String.pluralize(count: Int): String {
    if (count % 10 == 1 && count % 100 != 11) {
        return this
    }

    return "${this}s"
}

fun String.shorten(maxLength: Int) : String {
    if (this.length < maxLength){
        return this
    }

    return this.substring(0, maxLength) + "..."
}