using Autofac;
using CommandLine;
using TagCloud;

namespace TagCloudClient;

internal static class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<ConsoleOptions>(args)
            .WithParsed(settings =>
            {
                var container = SettingsBuilder.BuildContainer(settings);
                var generator = container.Resolve<CloudGenerator>();
                Console.WriteLine("File saved in " + generator.GenerateTagCloud());
            });
    }
}