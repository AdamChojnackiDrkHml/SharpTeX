using static SharpTeX.Extensions.StringExtensions;

namespace SharpTeX.Test.TeXBlock.ListBlock;

public class ListBlockTests
{
    [Fact]
    public void Render_EmptyItemize_ReturnsCorrectString()
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        var expected = """
                       \begin{itemize}
                       \end{itemize}
                       """;
        
        // Act
        var result = listBlock.Render();
                
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void Render_EmptyEnumerate_ReturnsCorrectString()
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewEnumerate();
        var expected = """
                       \begin{enumerate}
                       \end{enumerate}
                       """;
        
        // Act
        var result = listBlock.Render();
                
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void Render_ItemizeWithItems_ReturnsCorrectString()
    {
        // Arrange
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize()
            .AddItem("Item 1")
            .AddItem("Item 2");
        var expected = @"\begin{itemize}".NewLine() 
                       + @"\item Item 1".NewLine().Indent() 
                       + @"\item Item 2".NewLine().Indent() 
                       + @"\end{itemize}";   
        
        // Act
        var result = listBlock.Render();
        
        // Assert
        Assert.Equal(expected, result);
    }
}