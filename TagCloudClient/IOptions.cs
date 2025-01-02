using System.Drawing;
using System.Globalization;
using System.Text;
using CommandLine;

namespace TagCloudClient;

public interface IOptions
{
    public Size Size { get; set; }
    public string Path { get; set; }
    public Point Center { get; set; }
    public double Radius { get; set; }
    
    public FontFamily Font { get; set; }
    public string ImageName { get; set; }
    public Encoding UsingEncoding { get; }
    public string ImageFormat { get; set; }
    public double AngleOffset { get; set; }
    public CultureInfo Culture { get; set; }
    public string EncodingName { get; set; }
    public Color BackgroundColor { get; set; }
    public Color ForegroundColor { get; set; }
}