using Autofac;
using TagCloud;
using TagCloud.CloudLayouter;
using TagCloud.CloudLayouter.PointLayouter;
using TagCloud.CloudLayouter.PointLayouter.Generators;
using TagCloud.ImageGenerator;
using TagCloud.ImageSaver;
using TagCloud.WordsFilter;
using TagCloud.WordsReader;
using TagCloud.WordsReader.Readers;

namespace TagCloudClient;

public static class SettingsBuilder
{
    public static IContainer BuildContainer(IOptions settings)
    {
        var builder = new ContainerBuilder();

        RegisterSettings(builder, settings);
        RegisterLayouters(builder, settings);
        RegisterWordsReaders(builder, settings);
        RegisterWordsFilters(builder, settings);

        builder.RegisterType<CloudGenerator>().AsSelf();
        builder.RegisterType<BitmapGenerator>().AsSelf();
        builder.RegisterType<BitmapFileSaver>().As<IImageSaver>();

        return builder.Build();
    }

    private static void RegisterSettings(ContainerBuilder builder, IOptions settings)
    {
        builder.RegisterInstance(SettingsFactory.BuildBitmapSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildFileSaveSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildCsvReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildWordReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildFileReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildFermatSpiralSettings(settings)).AsSelf();
        builder.Register(context => SettingsFactory.BuildPointLayouterSettings(
            settings, context.Resolve<IPointsGenerator>())).AsSelf();
    }

    private static void RegisterWordsReaders(ContainerBuilder builder, IOptions settings)
    {
        builder
            .RegisterType<FileReader>().As<BaseFileReader>()
            .OnlyIf(_ => Path.GetExtension(settings.Path) == ".txt");

        builder
            .RegisterType<CsvFileReader>().As<BaseFileReader>()
            .OnlyIf(_ => Path.GetExtension(settings.Path) == ".csv");

        builder
            .RegisterType<WordFileReader>().As<BaseFileReader>()
            .OnlyIf(_ => Path.GetExtension(settings.Path) == ".docx");
    }

    private static void RegisterWordsFilters(ContainerBuilder builder, IOptions settings)
    {
        builder.RegisterType<LowercaseFilter>().As<IWordsFilter>();
        builder.RegisterType<BoringWordsFilter>().As<IWordsFilter>();
    }

    private static void RegisterLayouters(ContainerBuilder builder, IOptions settings)
    {
        builder
            .RegisterType<FermatSpiralPointsGenerator>().As<IPointsGenerator>();

        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
    }
}