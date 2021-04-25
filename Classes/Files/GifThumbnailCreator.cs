using System.IO;
using Microsoft.AspNetCore.Http;
using ImageMagick;
using Menhera.Intefaces;

namespace Menhera.Classes.Files
{
    public class GifThumbnailCreator : ThumbnailCreator, IThumbnailCreator
    {
        private string GifInfo { get; set; }
        private MagickImage Gif { get; }
        
        public GifThumbnailCreator(IFormFile file, string fileDirectory, string thumbNailDirectory) : base(file, fileDirectory, thumbNailDirectory)
        {
            Gif = new MagickImage(this.FileFullPath);
        }

        public void CreateThumbnail(int width = Constants.Constants.THUMBNAIL_WIDTH,
            int height = Constants.Constants.THUMBNAIL_HEIGHT)
        {
            using (Gif)
            {
                using (Stream stream = new FileStream(this.FileFullPath, FileMode.Create))
                {
                    File.CopyTo(stream);
                }

                if (Gif.Width < width && Gif.Height < height)
                {
                    using (Stream thumbnailSaveStream = new FileStream(ThumbnailFullPath, FileMode.Create))
                    {
                        File.CopyToAsync(thumbnailSaveStream);
                    }
                }
                else
                {
                    var size = new MagickGeometry(width, height);

                    Gif.Resize(size);
                    Gif.Write(this.ThumbnailFullPath);

                    this.GifInfo = $"{Gif.BaseWidth}x{Gif.BaseHeight}";

                    this.FileInfo = new GifInformation(this.FileName, this.ThumbnailName, GifInfo);
                }
            }
        }
    }
}