namespace Menhera.Classes.Files
{
    public class ImageInformation
    {
        public string FileName { get; }
        public string ThumbnailName { get; }
        public string Information { get; }

        public ImageInformation(string fileName, string thumbnailName, string info)
        {
            FileName = fileName;
            ThumbnailName = thumbnailName;
            Information = info;
        }
    }
}