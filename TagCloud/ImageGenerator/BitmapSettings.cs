using System.Drawing;

namespace TagCloud.ImageGenerator;

public record BitmapSettings(
    Size Sizes,
    FontFamily Font,
    Color BackgroundColor, 
    Color ForegroundColor);