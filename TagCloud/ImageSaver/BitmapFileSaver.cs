using System.Drawing;
using ResultTools;

namespace TagCloud.ImageSaver;

#pragma warning disable CA1416
public class BitmapFileSaver(string imageName, string imageFormat) : IImageSaver
{
    private readonly List<string> supportedFormats = ["png", "jpg", "jpeg", "bmp"];

    public BitmapFileSaver(FileSaveSettings settings)
        : this(settings.ImageName, settings.ImageFormat)
    { }

    public Result<string> Save(Bitmap image) 
        => !supportedFormats.Contains(imageFormat) 
            ? Result.Fail<string>($"Unsupported image format: {imageFormat}")
            : $"{imageName}.{imageFormat}".AsResult().Then(n => SaveImage(image, n));

    private static string SaveImage(Bitmap image, string name)
    {
        image.Save(name);
        return Path.Combine(Directory.GetCurrentDirectory(), name);
    }
}
#pragma warning restore CA1416