using System.Drawing;

namespace TagCloud.ImageSaver;

public interface IImageSaver
{
    public string Save(Bitmap image);
}