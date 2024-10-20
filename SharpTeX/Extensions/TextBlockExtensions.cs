using SharpTeX.TeXBlock.SimpleBlock.TextBlock;

namespace SharpTeX.Extensions;

public static class TextBlockExtensions
{
    public static TextBlock ToTextBlock(this string content)
    {
        return TextBlock.CreateTextBlock(content);
    }
    
}