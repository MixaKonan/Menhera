using Menhera.Classes.Constants;

namespace Menhera.Intefaces
{
    public interface IThumbnailCreator
    {
        public void CreateThumbnail(int width = Constants.THUMBNAIL_WIDTH, int height = Constants.THUMBNAIL_HEIGHT);
    }
}