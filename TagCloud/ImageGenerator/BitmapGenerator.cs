using System.Drawing;
using TagCloud.CloudLayouter;

namespace TagCloud.ImageGenerator;

#pragma warning disable CA1416
public class BitmapGenerator(Size size, FontFamily family, Color background, Color foreground, ICloudLayouter layouter)
{
    public BitmapGenerator(BitmapSettings settings, ICloudLayouter layouter)
        : this(settings.Sizes, settings.Font, settings.BackgroundColor, settings.ForegroundColor, layouter)
    {}
    
    public Bitmap GenerateWindowsBitmap(List<WordTag> tags)
    {
        var bitmap = new Bitmap(size.Width, size.Height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(background);
        var brush = new SolidBrush(foreground);

        foreach (var tag in tags)
        {
            var font = new Font(family, tag.FontSize);
            var wordSize = CeilSize(graphics.MeasureString(tag.Word, font));
            
            var positionRect = layouter.PutNextRectangle(wordSize);
            graphics.DrawString(tag.Word, font, brush, positionRect);
        }

        return bitmap;
    }

    private static Size CeilSize(SizeF size)
        => new((int)size.Width + 1, (int)size.Height + 1);
}
#pragma warning restore CA1416