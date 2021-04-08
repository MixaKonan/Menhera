using System;
using System.Text.RegularExpressions;
using Menhera.Models;

namespace Menhera.Classes.PostFormatting
{
    public static class PostFormatter
    {
        private static Regex _lineBreakRegex;
        private static Regex _postReferenceRegex;
        private static Regex _postBoldTextRegex;
        private static Regex _postItalicTextRegex;
        private static Regex _postUnderlinedTextRegex;
        private static Regex _postIncorrectTextRegex;
        private static Regex _postQuoteTextRegex;
        private static Regex _postHtmlTagsRegex;
        
        public static string GetFormattedPostText(Post post)
        {
            _lineBreakRegex = new Regex(Constants.Constants.HTML_POST_END_LINE_BREAK_PATTERN, RegexOptions.Compiled);
            _postReferenceRegex = new Regex(Constants.Constants.HTML_POST_REFERENCE_PATTERN, RegexOptions.Compiled);
            _postBoldTextRegex = new Regex(Constants.Constants.HTML_POST_BOLD_TEXT_PATTERN, RegexOptions.Compiled);
            _postItalicTextRegex = new Regex(Constants.Constants.HTML_POST_ITALIC_TEXT_PATTERN, RegexOptions.Compiled);
            _postUnderlinedTextRegex = new Regex(Constants.Constants.HTML_POST_UNDERLINED_TEXT_PATTERN, RegexOptions.Compiled);
            _postIncorrectTextRegex = new Regex(Constants.Constants.HTML_POST_INCORRECT_TEXT_PATTERN, RegexOptions.Compiled);
            _postQuoteTextRegex = new Regex(Constants.Constants.HTML_POST_QUOTE_TEXT_PATTERN,   RegexOptions.Compiled | RegexOptions.Multiline);
            
            var breaksTrimmed = _lineBreakRegex.Replace(post.Comment, "");
            
            var referenceAdded = _postReferenceRegex.Replace(breaksTrimmed,
                $"<a href=\"/Thread/Thread/{post.ThreadId}#$2\" class=\"post-link\" id=\"post-link-$2\">$1$2</a>");
            
            var boldAdded = _postBoldTextRegex.Replace(referenceAdded, "<b>$1</b>");
            var italicAdded = _postItalicTextRegex.Replace(boldAdded, "<i>$1</i>");
            var underlinedAdded = _postUnderlinedTextRegex.Replace(italicAdded, "<u>$1</u>");
            var incorrectAdded = _postIncorrectTextRegex.Replace(underlinedAdded, "<s>$1</s>");

            var quoteAdded = _postQuoteTextRegex.Replace(incorrectAdded,
                "<span style=\"color: #789922;\">$1$2</span>");

            return quoteAdded;
        }

        public static string GetHtmlTrimmedComment(Post post)
        {
            _postHtmlTagsRegex = new Regex(Constants.Constants.HTML_TAGS_TEXT_PATTERN, RegexOptions.Compiled);

            return _postHtmlTagsRegex.Replace(post.Comment, string.Empty);
        }
    }
}