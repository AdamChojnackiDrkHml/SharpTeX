using SharpTeX.Extensions;
using SharpTeX.TeXBlock.Document;

namespace SharpTeX.Test.TeXBlock.Document;

public class DocumentTests
{
    // [Fact]
    // public void Render_EmptyDocument_ReturnsCorrectString()
    // {
    //     // Arrange
    //     var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
    //     var expected = @"\begin{document}".NewLine() 
    //                    + @"\maketitle".NewLine().Indent() 
    //                    + @"\end{document}";
    //     
    //     // Act
    //     var result = document.Render();
    //     
    //     // Assert
    //     Assert.Equal(expected, result);
    // }
    //
    // [Fact]
    // public void Render_DocumentWithList_ReturnsCorrectString()
    // {
    //     // Arrange
    //     var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
    //     
    //     var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize()
    //         .AddItem("Item 1")
    //         .AddItem("Item 2");
    //     
    //     document.AddBlock(listBlock);
    //     
    //     var expected = @"\begin{document}".NewLine() 
    //                    + @"\maketitle".NewLine().Indent() 
    //                    + @"\begin{itemize}".NewLine().Indent() 
    //                    + @"\item Item 1".NewLine().Indent(2) 
    //                    + @"\item Item 2".NewLine().Indent(2) 
    //                    + @"\end{itemize}".NewLine().Indent() 
    //                    + @"\end{document}";
    //     
    //     // Act
    //     var result = document.Render();
    //     
    //     // Assert
    //     Assert.Equal(expected, result);
    // }
}