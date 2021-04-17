using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Menhera.Classes.Files
{
    public abstract class ThumbnailCreator
    {
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

            FileName = string.Concat(Guid.NewGuid().ToString(), FileExtension);

            FileFullPath = Path.Combine(FileDirectory, FileName ?? throw new InvalidOperationException());

            ThumbnailDirectory = thumbNailDirectory;

            ThumbnailName = string.Concat("thmb-", FileName);

            ThumbnailFullPath = Path.Combine(ThumbnailDirectory, ThumbnailName);
        }

        public virtual void CreateThumbnail(double width = Constants.Constants.THUMBNAIL_WIDTH,
            double height = Constants.Constants.THUMBNAIL_HEIGHT)
        {

        }
    }
}