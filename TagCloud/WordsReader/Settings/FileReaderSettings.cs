using System.Text;

namespace TagCloud.WordsReader.Settings;

public record FileReaderSettings(string FilePath, Encoding Encoding);