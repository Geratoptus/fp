using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using ResultTools;
using TagCloud.WordsReader.Settings;

namespace TagCloud.WordsReader.Readers;

public class CsvFileReader(string path, CultureInfo cultureInfo) : BaseFileReader(path)
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private class TableCell(string word)
    {
        [Index(0)]
        public string Word { get; set; } = word;
    }

    public CsvFileReader(CsvFileReaderSettings settings)
        : this(settings.FilePath, settings.Culture)
    { }

    protected override Result<List<string>> ReadFromExistingFile(string path)
    {
        try
        {
            var configuration = new CsvConfiguration(cultureInfo)
            {
                HasHeaderRecord = false
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, configuration);
            return csv.GetRecords<TableCell>().Select(cell => cell.Word).ToList();
        }
        catch (IOException)
        {
            return Result.Fail<List<string>>($"Can't open file {path}");
        }
        catch (CultureNotFoundException)
        {
            return Result.Fail<List<string>>("Something wrong with culture");
        }
        catch (ArgumentException)
        {
            return Result.Fail<List<string>>("Wrong arguments for configuration");
        }
    }
}