using ResultTools;

namespace TagCloud.WordsReader.Readers;

public abstract class BaseFileReader(string path)
{
    public Result<List<string>> ReadWords()
        => File.Exists(path)
            ? ReadFromExistingFile(path)
            : Result.Fail<List<string>>("No file found");

    protected abstract Result<List<string>> ReadFromExistingFile(string path);
}