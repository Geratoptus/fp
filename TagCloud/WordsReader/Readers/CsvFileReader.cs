using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using TagCloud.WordsReader.Settings;

namespace TagCloud.WordsReader.Readers;

public class CsvFileReader(string path, CultureInfo cultureInfo) : IWordsReader
{
    private class TableCell
    {
        [Index(0)]
        public string Word { get; set; }
    }

    public CsvFileReader(CsvFileReaderSettings settings)
        : this(settings.FilePath, settings.Culture)
    { }

    public List<string> ReadWords()
    {
        var configuration = new CsvConfiguration(cultureInfo)
        {
            HasHeaderRecord = false
        };

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, configuration);
        return csv.GetRecords<TableCell>().Select(cell => cell.Word).ToList();
    }

}