namespace SharpTeX.TeXBlock.SimpleBlock;

public abstract class SimpleBlock : Block
{
    public abstract string GetContent();
    
    public abstract SimpleBlock SetContent(string content);
}