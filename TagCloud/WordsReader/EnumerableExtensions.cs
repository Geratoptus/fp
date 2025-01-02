namespace TagCloud.WordsReader;

public static class EnumerableExtensions

{
    public static string ToText(this IEnumerable<string> words, string separator = "")
        => string.Join(separator, words);
}