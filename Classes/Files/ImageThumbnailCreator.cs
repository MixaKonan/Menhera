using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Menhera.Classes.Files
{
    public class ImageThumbnailCreator : ThumbnailCreator
    {
        private string ImageInfo { get; set; }
        private Image Image { get; set; }

        public ImageThumbnailCreator(IFormFile file, string fileDirectory, string thumbNailDirectory) : base(file, fileDirectory, thumbNailDirectory)
        {
        }

        public override void CreateThumbnail(double width = Constants.Constants.THUMBNAIL_WIDTH, double height = Constants.Constants.THUMBNAIL_HEIGHT)
        {
            using (Stream fileSaveStream = new FileStream(FileFullPath, FileMode.Create))
            {
                File.CopyTo(fileSaveStream);
            }
            
            Image = Image.FromFile(FileFullPath);

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
                double coefficient =
                    Image.Width > Image.Height
                        ? width / Image.Width
                        : height / Image.Height;

                int thumbnailW = (int) Math.Round(Image.Width * coefficient, MidpointRounding.ToEven);
                int thumbnailH = (int) Math.Round(Image.Height * coefficient, MidpointRounding.ToEven);

                Image thumbnail = Image.GetThumbnailImage(thumbnailW, thumbnailH,
                    () => false,
                    IntPtr.Zero);

                using (Stream thumbnailSaveStream = new FileStream(ThumbnailFullPath, FileMode.Create))
                {
                    switch (FileExtension)
                    {
                        case ".jpeg":
                        case ".jpg":
                            thumbnail.Save(thumbnailSaveStream, ImageFormat.Jpeg);
                            break;
                        case ".png":
                            thumbnail.Save(thumbnailSaveStream, ImageFormat.Png);
                            break;
                    }
                }
                
                FileInfo = new ImageInformation(FileName, ThumbnailName, ImageInfo);
            }
        }
    }
}