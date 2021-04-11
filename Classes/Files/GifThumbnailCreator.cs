using Microsoft.AspNetCore.Http;

namespace Menhera.Classes.Files
{
    public class GifThumbnailCreator : ThumbnailCreator
    {
        public GifThumbnailCreator(IFormFile file, string fileDirectory, string thumbNailDirectory) : base(file, fileDirectory, thumbNailDirectory)
        {
            
        }
    }
}