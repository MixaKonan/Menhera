using System.IO;
using Microsoft.AspNetCore.Http;
using ImageMagick;

namespace Menhera.Classes.Files
{
    public class ImageThumbnailCreator : ThumbnailCreator
    {
        private string ImageInfo { get; set; }
        private MagickImage Image { get; set; }

        public ImageThumbnailCreator(IFormFile file, string fileDirectory, string thumbNailDirectory) : base(file, fileDirectory, thumbNailDirectory)
        {
            
        }

        public override void  CreateThumbnail(int width = Constants.Constants.THUMBNAIL_WIDTH, int height = Constants.Constants.THUMBNAIL_HEIGHT)
        {
            using (Stream fileSaveStream = new FileStream(FileFullPath, FileMode.Create))
            {
                File.CopyTo(fileSaveStream);
            }

            using (Image = new MagickImage(this.FileFullPath))
            {
                ImageInfo = $"{Image.Width}x{Image.Height}";

                if (Image.Width < width && Image.Height < height)
                {
                    using (Stream thumbnailSaveStream = new FileStream(ThumbnailFullPath, FileMode.Create))
                    {
                        File.CopyToAsync(thumbnailSaveStream);
                    }
                }
                else
                {
                    var size = new MagickGeometry(width, height);

                    Image.Resize(size);
                    Image.Write(this.ThumbnailFullPath);


                    FileInfo = new ImageInformation(FileName, ThumbnailName, ImageInfo);
                }
            }
        }
    }
}