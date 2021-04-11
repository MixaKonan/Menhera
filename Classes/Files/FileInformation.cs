namespace Menhera.Classes.Files
{
    public class FileInformation
    {
        public string FileName { get; }
        public string ThumbnailName { get; }
        public string Information { get; }

        protected FileInformation(string fileName, string thumbnailName, string info)
        {
            FileName = fileName;
            ThumbnailName = thumbnailName;
            Information = info;
        }
    }
}