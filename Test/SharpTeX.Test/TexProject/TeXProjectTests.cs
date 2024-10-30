using FluentAssert;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Moq;
using SharpTeX.Extensions;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;
using SharpTeX.TeXBlock.Document;
using SharpTeX.TeXBlock.ListBlock;
using static SharpTeX.Test.Utilities.LoggerUtilities;

namespace SharpTeX.Test.TexProject;

public class TeXProjectTests
{
    [Fact]
    public void AddAuthor_AuthorNotSet_AuthorIsSet()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var project = TeXProject.TeXProject.CreateTeXProject("Title", logger.Object);
        var author = "Author";

        // Act
        var res = project.ChangeAuthor(author);

        // Assert
        res.ShouldBeSameInstanceAs(project);
        res.Author.Should().Be(author);
    }

    [Fact]
    public void AddAuthor_AuthorSet_AuthorIsChanged()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var project = TeXProject.TeXProject.CreateTeXProject("Title", logger.Object, "Author");
        var author = "New Author";

        // Act
        var res = project.ChangeAuthor(author);

        // Assert
        res.ShouldBeSameInstanceAs(project);
        res.Author.Should().Be(author);
    }

    [Fact]
    public void SetDocumentClass_DefaultDocumentClassSet_DocumentClassIsChanged()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var project = TeXProject.TeXProject.CreateTeXProject("Title", logger.Object);
        var documentClass = "presentation";

        // Act
        var res = project.SetDocumentClass(documentClass);

        // Assert
        res.ShouldBeSameInstanceAs(project);
        res.DocumentClass.Should().Be(documentClass);
    }

    [Fact]
    public void AddDocument_NoDocumentSet_DocumentIsSet()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var project = TeXProject.TeXProject.CreateTeXProject("Title", logger.Object);
        var document = Document.CreateDocument();

        // Act
        var res = project.AddDocument(document);

        // Assert
        res.ShouldBeSameInstanceAs(project);
        res.Document.Should().Be(document);
    }

    [Fact]
    public void Render_EmptyProject_FailureLogged()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var project = TeXProject.TeXProject.CreateTeXProject("Title", logger.Object);
        var renderer = new Mock<IRenderer>();

        // Act
        var res = project.Render(renderer.Object);

        // Assert
        res.IsSuccess.Should().BeFalse();
        VerifyLog(logger, LogLevel.Error, "TeXProject: Document is not set.", Times.Once());
    }

    [Fact]
    public void Render_ProjectWithDocument_Success()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var project = TeXProject.TeXProject.CreateTeXProject("TitleOfDocument", logger.Object);
        var renderer = new Mock<IRenderer>();
        var document = Document.CreateDocument()
            .AddBlock(
                ListBlock.NewItemize()
                    .AddItem("Item 1")
                    .AddItem("Item 2")
            )
            .AddTitle();
        project.AddDocument(document);
        var preambule = @"\documentclass{article}" + Environment.NewLine 
                            + @"\usepackage[utf8]{inputenc}" + Environment.NewLine 
                            + @"\usepackage[english]{babel}" + Environment.NewLine 
                            + @"\usepackage[a4paper,top=2cm,bottom=2cm,left=3cm,right=3cm,marginparwidth=1.75cm]{geometry}" + Environment.NewLine 
                            + @"\title{TitleOfDocument}" + Environment.NewLine;
        // Act
        var res = project.Render(renderer.Object);
        
        // Assert
        res.IsSuccess.Should().BeTrue();
        renderer.Verify(mock => mock.SetRootBlock(It.IsAny<RenderedBlock>()));
        renderer.Verify(mock => mock.AddSimpleBlock(null), Times.Exactly(4));
        renderer.Verify(mock => mock.AddToBlock(It.IsAny<RenderedBlock>(), "\\item Item 1"), Times.Once);
        renderer.Verify(mock => mock.AddToBlock(It.IsAny<RenderedBlock>(), "\\maketitle"), Times.Once);
        renderer.Verify(mock => mock.AddToBlock(It.IsAny<RenderedBlock>(), "\\item Item 2"), Times.Once);
        renderer.Verify(mock => mock.AddToBlock(It.IsAny<RenderedBlock>(), preambule), Times.Once);
        renderer.Verify(mock => mock.AddToBlock(null, (RenderedBlock?)null), Times.Exactly(5));
        renderer.Verify(mock => mock.AddNamedBlock("document", null), Times.Once);
        renderer.Verify(mock => mock.AddNamedBlock("itemize", null), Times.Once);
        renderer.Verify(mock => mock.AddSimpleBlock(It.IsAny<string>()), Times.Exactly(4));
        renderer.VerifyNoOtherCalls();

    }

}