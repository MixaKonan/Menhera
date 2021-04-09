namespace Menhera.Classes.Constants
{
    public static class Constants
    {
        public const double THUMBNAIL_WIDTH = 200;

        public const double THUMBNAIL_HEIGHT = 200;

        public const string HTML_POST_REFERENCE_PATTERN = @"([>]{2})(\d+)";

        public const string HTML_POST_END_LINE_BREAK_PATTERN = @"[\n|\r|\r\n]+$";

        public const string HTML_POST_BOLD_TEXT_PATTERN = @"\[b\](.+)\[/b\]";

        public const string HTML_POST_ITALIC_TEXT_PATTERN = @"\[i\](.+)\[/i\]";

        public const string HTML_POST_UNDERLINED_TEXT_PATTERN = @"\[u\](.+)\[/u\]";

        public const string HTML_POST_INCORRECT_TEXT_PATTERN = @"\[s\](.+)\[/s\]";
        
        public const string HTML_POST_SPOILER_TEXT_PATTERN = @"\[spoiler\](.+)\[/spoiler\]";

        public const string HTML_POST_QUOTE_TEXT_PATTERN = @"^(>)\s*(.+)$";

        public const string HTML_TAGS_TEXT_PATTERN = @"<.*?>";

        public const int BOARD_PAGE_SIZE = 10;
    }
}