using Autofac;
using CommandLine;
using ResultTools;
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
                container.Resolve<CloudGenerator>().GenerateTagCloud()
                    .Then(p => Console.WriteLine("File saved in " + p))
                    .OnFail(err => Console.WriteLine("Generating finished with error: " + err));
            });
    }
}