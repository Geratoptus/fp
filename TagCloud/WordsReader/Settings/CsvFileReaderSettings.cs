using System.Globalization;

namespace TagCloud.WordsReader.Settings;

public record CsvFileReaderSettings(string FilePath, CultureInfo Culture);