using FluentAssertions.CSharpFunctionalExtensions;
using SharpTeX.Extensions;

namespace SharpTeX.Test.TexProject;

public class TeXProjectTests
{
    [Fact]
    public void Render_EmptyProject_ReturnsCorrectString()
    {
        // Arrange
        var title = "The Title";
        var author = "My Author";
        var project = SharpTeX.TeXProject.TeXProject.CreateTeXProject(title, author);
        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
        
        project.AddDocument(document);
        
        var expected = @$"\documentclass{{article}}".NewLine()
                       + @"\usepackage[english]{babel}".NewLine()
                       + @"\usepackage[a4paper,top=2cm,bottom=2cm,left=3cm,right=3cm,marginparwidth=1.75cm]{geometry}".NewLine()
                       + $@"\title{{{title}}}".NewLine()
                       + $@"\author{{{author}}}".NewLine()
                       + @"\begin{document}".NewLine()
                       + @"\end{document}"; 
        
        
        // Act
        var result = project.Render();
        
        // Assert
        result.Should().BeSuccess(expected);
    }
    
    [Fact]
    public void Render_ProjectWithoutDocument_ReturnsFailure()
    {
        // Arrange
        var title = "The Title";
        var author = "My Author";
        var project = SharpTeX.TeXProject.TeXProject.CreateTeXProject(title, author);
        
        // Act
        var result = project.Render();
        
        // Assert
        result.Should().BeFailure("Document is not set.");
    }
    
    [Fact]
    public void Render_ProjectWithoutAuthor_ReturnsSuccess()
    {
        // Arrange
        var title = "The Title";
        var project = SharpTeX.TeXProject.TeXProject.CreateTeXProject(title);
        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
        
        project.AddDocument(document);
        
        var expected = @$"\documentclass{{article}}".NewLine()
                       + @"\usepackage[english]{babel}".NewLine()
                       + @"\usepackage[a4paper,top=2cm,bottom=2cm,left=3cm,right=3cm,marginparwidth=1.75cm]{geometry}".NewLine()
                       + $@"\title{{{title}}}".NewLine()
                       + @"\begin{document}".NewLine()
                       + @"\end{document}";
        
        // Act
        var result = project.Render();
        
        // Assert
        result.Should().BeSuccess(expected);
    }

    [Fact]
    public void Render_ProjectWithContent_ReturnsSuccess()
    {
        // Arrange
        var title = "The Title";
        var author = "My Author";

        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument().AddBlock(
            SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize()
                .AddItem("Item 1")
                .AddItem("Item 2")
        );
        
        var project = SharpTeX.TeXProject.TeXProject.CreateTeXProject(title, author)
            .AddDocument(document);
        
        var expected = @$"\documentclass{{article}}".NewLine()
                       + @"\usepackage[english]{babel}".NewLine()
                       + @"\usepackage[a4paper,top=2cm,bottom=2cm,left=3cm,right=3cm,marginparwidth=1.75cm]{geometry}".NewLine()
                       + $@"\title{{{title}}}".NewLine()
                       + $@"\author{{{author}}}".NewLine()
                       + @"\begin{document}".NewLine()
                       + @"\begin{itemize}".NewLine().Indent()
                       + @"\item Item 1".NewLine().Indent(2)
                       + @"\item Item 2".NewLine().Indent(2)
                       + @"\end{itemize}".NewLine().Indent()
                       + @"\end{document}";
        
        // Act
        var result = project.Render();
        
        // Assert
        result.Should().BeSuccess(expected);
    }
}