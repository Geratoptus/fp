using System.Drawing;
using System.Globalization;
using System.Text;
using CommandLine;

namespace TagCloudClient;

public class ConsoleOptions : IOptions
{
    [Value(0, Required = true, HelpText = "Source file path")]
    public string Path { get; set; } = "";

    [Option('e', "encoding",
        Required = false,
        HelpText = "Source encoding")]
    public string EncodingName { get; set; } = "utf-8";
    public Encoding UsingEncoding => Encoding.GetEncoding(EncodingName);

    [Option('s', "size",
        Required = false,
        HelpText = "Image size")]
    public Size Size { get; set; } = new(1920, 1080);

    [Option('f', "font",
        Required = false,
        HelpText = "Words font")]
#pragma warning disable CA1416
    public FontFamily Font { get; set; } = new("Arial");
#pragma warning restore CA1416

    [Option('b', "background-color",
        Required = false,
        HelpText = "Background color")]
    public Color BackgroundColor { get; set; } = Color.Black;

    [Option('c', "word-color",
        Required = false,
        HelpText = "Words color")]
    public Color ForegroundColor { get; set; } = Color.White;

    [Option("image-name", 
        Required = false, 
        HelpText = "Image name")]
    public string ImageName { get; set; } = "result";

    [Option("image-format",
        Required = false,
        HelpText = "Image format")]
    public string ImageFormat { get; set; } = "png";
    
    
    [Option("angle-offset",
        Required = false,
        HelpText = "Delta of the angle for the spiral.")]
    public double AngleOffset { get; set; } = 0.5;

    [Option("radius",
        Required = false,
        HelpText = "Distance between the turns of the spiral")]
    public double Radius { get; set; } = 1;

    [Option("center",
        Required = false,
        HelpText = "The center of the cloud in the image")]
    public Point Center { get; set; } = new(1920 / 2, 1080 / 2);
    
    [Option("culture", 
        Required = false, 
        HelpText = "CSV culture information")]
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
}