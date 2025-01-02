using System.Drawing;
using ResultTools;

namespace TagCloud.ImageSaver;

public interface IImageSaver
{
    public Result<string> Save(Bitmap image);
}