using TagCloud.CloudLayouter.PointLayouter.Generators;
using TagCloud.CloudLayouter.PointLayouter.Settings;
using TagCloud.CloudLayouter.PointLayouter.Settings.Generators;
using TagCloud.ImageGenerator;
using TagCloud.ImageSaver;
using TagCloud.WordsReader.Settings;

namespace TagCloudClient;

public static class SettingsFactory
{
    public static FileReaderSettings BuildFileReaderSettings(IOptions options)
        => new(options.Path, options.UsingEncoding);

    public static BitmapSettings BuildBitmapSettings(IOptions options)
        => new(options.Size, options.Font, options.BackgroundColor, options.ForegroundColor);
    
    public static FermatSpiralSettings BuildFermatSpiralSettings(IOptions options)
        => new(options.Radius, options.AngleOffset);

    public static PointLayouterSettings BuildPointLayouterSettings(IOptions options, IPointsGenerator generator)
        => new(options.Center, generator);
    
    public static WordFileReaderSettings BuildWordReaderSettings(IOptions options)
        => new(options.Path);

    public static CsvFileReaderSettings BuildCsvReaderSettings(IOptions options) 
        => new(options.Path, options.Culture);

    public static FileSaveSettings BuildFileSaveSettings(IOptions options)
        => new(options.ImageName, options.ImageFormat);
}