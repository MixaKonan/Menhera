using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Menhera.Classes
{
    public class ImageThumbnailCreator : IDisposable
    {
        public string FileName { get; }
        public string ThumbnailName { get; }
        public string ImageInfo { get; set; }
        private Image Image { get; set; }
        private IFormFile File { get; }
        private string FileExtension { get; }
        private string FileDirectory { get; }
        private string FileFullPath { get; }
        private string ThumbnailDirectory { get; }
        private string ThumbnailFullPath { get; }

        public ImageThumbnailCreator(IFormFile file, string fileDirectory, string thumbNailDirectory)
        {
            File = file;

            FileExtension = Path.GetExtension(file.FileName);

            FileDirectory = fileDirectory;

            FileName = file.FileName;

            FileFullPath = Path.Combine(FileDirectory, FileName ?? throw new InvalidOperationException());

            ThumbnailDirectory = thumbNailDirectory;

            ThumbnailName = string.Concat(new Random().Next(int.MaxValue).ToString(), Path.GetExtension(file.FileName));

            ThumbnailFullPath = Path.Combine(ThumbnailDirectory, ThumbnailName);
        }

        public async Task CreateThumbnail(double width, double height)
        {
            await using (Stream fileSaveStream = new FileStream(FileFullPath, FileMode.Create))
            {
                await File.CopyToAsync(fileSaveStream);
            }

            Image = Image.FromFile(FileFullPath);

            ImageInfo = $"{Image.Width}x{Image.Height}";

            if (Image.Width < width && Image.Height < height)
            {
                await using (Stream thumbnailSaveStream = new FileStream(ThumbnailFullPath, FileMode.Create))
                {
                    await File.CopyToAsync(thumbnailSaveStream);
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

                await using (Stream thumbnailSaveStream = new FileStream(ThumbnailFullPath, FileMode.Create))
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
            }
        }

        public void Dispose()
        {
            Image?.Dispose();
            
            GC.Collect();
        }

        ~ImageThumbnailCreator()
        {
            Image?.Dispose();
            
            GC.Collect();
        }
    }
}