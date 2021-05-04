using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Menhera.Classes.Files
{
    public abstract class ThumbnailCreator
    {
        private static Random random = new Random();
        protected string FileName { get; }
        protected string ThumbnailName { get; }
        public FileInformation FileInfo { get; set; }
        protected IFormFile File { get; }
        protected string FileExtension { get; }
        protected string FileDirectory { get; }
        protected string FileFullPath { get; }
        protected string ThumbnailDirectory { get; }
        protected string ThumbnailFullPath { get; }

        protected ThumbnailCreator(IFormFile file, string fileDirectory, string thumbNailDirectory)
        {
            File = file;

            FileExtension = Path.GetExtension(file.FileName);

            FileDirectory = fileDirectory;

            FileName = string.Concat(GetRandomString(8), FileExtension);

            FileFullPath = Path.Combine(FileDirectory, FileName ?? throw new InvalidOperationException());

            ThumbnailDirectory = thumbNailDirectory;

            ThumbnailName = string.Concat("th-", FileName);

            ThumbnailFullPath = Path.Combine(ThumbnailDirectory, ThumbnailName);
        }

        public abstract void CreateThumbnail(int width = Constants.Constants.THUMBNAIL_WIDTH, int height = Constants.Constants.THUMBNAIL_HEIGHT);
        
        private static string GetRandomString(int length)
        {
            const string chars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVNM0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}