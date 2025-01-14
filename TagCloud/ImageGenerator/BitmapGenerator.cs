using System.Drawing;
using ResultTools;
using TagCloud.CloudLayouter;

namespace TagCloud.ImageGenerator;

#pragma warning disable CA1416
public class BitmapGenerator(Size size, FontFamily family, Color background, Color foreground, ICloudLayouter layouter)
{
    private readonly SolidBrush brush = new(foreground);
    
    public BitmapGenerator(BitmapSettings settings, ICloudLayouter layouter)
        : this(settings.Sizes, settings.Font, settings.BackgroundColor, settings.ForegroundColor, layouter)
    {}
    
    public Result<Bitmap> GenerateWindowsBitmap(IEnumerable<WordTag> tags)
    {
        var bitmap = size is { Width: > 0, Height: > 0 } 
            ? new Bitmap(size.Width, size.Height).AsResult() 
            : Result.Fail<Bitmap>("Cannot generate bitmap with negative size");

        return bitmap.Then(b =>
        {
            using var graphics = Graphics.FromImage(b); 
            graphics.Clear(background);
            var result = ProcessTags(tags, graphics)
                .FirstOrDefault(r => !r.IsSuccess, Result.Ok());
            return result.IsSuccess ? bitmap : Result.Fail<Bitmap>(result.Error);
        });
    }

    private Result<Font> BuildFont(int fontSize) 
        => fontSize > 0 
            ? new Font(family, fontSize).AsResult() 
            : Result.Fail<Font>("Cannot generate font with negative size");

    private IEnumerable<Result<None>> ProcessTags(IEnumerable<WordTag> tags, Graphics graphics)
        => tags.Select(t => BuildFont(t.FontSize).Then(f => DrawTag(f, t, graphics)));

    private Result<None> DrawTag(Font? font, WordTag tag, Graphics graphics)
        => font.AsResult()
            .Then(f => CeilSize(graphics.MeasureString(tag.Word, f)))
            .Then(layouter.PutNextRectangle)
            .Then(FitsInRange)
            .Then(r => graphics.DrawString(tag.Word, font, brush, r));

    private Result<Rectangle> FitsInRange(Rectangle rect)
        => new Rectangle(Point.Empty, size).Contains(rect) 
            ? Result.Ok(rect) 
            : Result.Fail<Rectangle>("Cannot fit in the given rectangle");

    private static Size CeilSize(SizeF size)
        => new((int)size.Width + 1, (int)size.Height + 1);
}
#pragma warning restore CA1416