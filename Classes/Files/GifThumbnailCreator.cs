using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using ImageMagick;

namespace Menhera.Classes.Files
{
    public class GifThumbnailCreator : ThumbnailCreator
    {
        private string GifInfo { get; set; }
        
        public GifThumbnailCreator(IFormFile file, string fileDirectory, string thumbNailDirectory) : base(file, fileDirectory, thumbNailDirectory)
        {
        }
        
        public override void CreateThumbnail(double width = Constants.Constants.THUMBNAIL_WIDTH, double height = Constants.Constants.THUMBNAIL_HEIGHT)
        {
            using (Stream stream = new FileStream(this.FileFullPath, FileMode.Create))
            {
                File.CopyTo(stream);
            }

            using (var image = new MagickImage(this.FileFullPath))
            {
                var size = new MagickGeometry((int)width, (int)height);
                
                image.Resize(size);
                
                image.Write(this.ThumbnailFullPath);

                this.GifInfo = $"{image.BaseWidth}x{image.BaseHeight}";
                
                this.FileInfo = new GifInformation(this.FileName, this.ThumbnailName, GifInfo);
            }
        }
    }
}