using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;
using ImageMagick;
using Menhera.Intefaces;

namespace Menhera.Classes.Files
{
    public class ImageThumbnailCreator : ThumbnailCreator, IThumbnailCreator
    {
        private string ImageInfo { get; set; }
        private MagickImage Image { get; }

        public ImageThumbnailCreator(IFormFile file, string fileDirectory, string thumbNailDirectory) : base(file, fileDirectory, thumbNailDirectory)
        {
            Image = new MagickImage(this.FileFullPath);
        }

        public void CreateThumbnail(int width = Constants.Constants.THUMBNAIL_WIDTH, int height = Constants.Constants.THUMBNAIL_HEIGHT)
        {
            using (Stream fileSaveStream = new FileStream(FileFullPath, FileMode.Create))
            {
                File.CopyTo(fileSaveStream);
            }

            using (Image)
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