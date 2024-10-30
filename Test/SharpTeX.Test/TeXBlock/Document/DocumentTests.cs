using System.Collections.ObjectModel;
using FluentAssert;
using FluentAssertions;
using Moq;
using SharpTeX.Renderer;
using SharpTeX.Renderer.Models;
using SharpTeX.TeXBlock.SimpleBlock.TextBlock;

namespace SharpTeX.Test.TeXBlock.Document;

public class DocumentTests
{
    [Fact]
    public void CreateDocument_Nothing_ReturnsCorrectDocument()
    {
        // Act
        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
        
        // Assert
        document.Should().NotBeNull();
        document.BlockName.Should().Be("document");
        document.GetDocumentPreContent().ShouldBeEmpty();
    }

    [Fact]
    public void AddTitle_NewDocument_TitleCommandIsAdded()
    {
        // Arrange
        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
        
        // Act
        var res = document.AddTitle();
        
        // Assert
        res.ShouldBeSameInstanceAs(document);
        res.GetDocumentPreContent().Should().ContainSingle();
        res.GetDocumentPreContent().Should().Contain(@"\maketitle");
    }
    
    [Fact]
    public void GetDocumentPreContent_NonEmptyPreContent_ReturnsReadOnlyCollection()
    {
        // Arrange
        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
        document.AddTitle();
        
        // Act
        var res = document.GetDocumentPreContent();
        
        // Assert
        res.Should().NotBeEmpty();
        res.Should().BeOfType<ReadOnlyCollection<string>>();
    }
    
    [Theory]
    [MemberData(nameof(AddBlockTestCases))]
    public void AddBlock_VariousBlocks_BlockIsAddedCorrectly(
        SharpTeX.TeXBlock.Block block,
        SharpTeX.TeXBlock.Document.Document document,
        int expectedNoOfObjects,
        string testcase
    )
    {
        // Act
        var res = document.AddBlock(block);
        
        // Assert
        res.ShouldBeSameInstanceAs(document);
        res.GetChildren().Should().Contain(block);
        res.GetChildren().Count.Should().Be(expectedNoOfObjects);
    }

    [Fact]
    public void Render_EmptyDocument_RendererIsCorrectlyCalled()
    {
        // Arrange
        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
        var mock = new Mock<IRenderer>();
        
        // Act
        var result = document.Render(mock.Object);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        mock.Verify(x => x.AddNamedBlock(document.BlockName, null), Times.Once);
        mock.Verify(x => x.AddToBlock(result.Value, ""), Times.Once);
        mock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Render_DocumentWithSingleElement_CorrectInvocations()
    {
        // Arrange
        var mock = new Mock<IRenderer>();
        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize();
        document.AddBlock(listBlock);
        
        // Act
        var result = document.Render(mock.Object);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        mock.Verify(x => x.AddNamedBlock(document.BlockName, null), Times.Once);
        mock.Verify(x => x.AddToBlock(result.Value, ""), Times.Once);
        mock.Verify(x => x.AddNamedBlock(listBlock.BlockName, null), Times.Once);
        mock.Verify(x => x.AddToBlock(result.Value, It.IsAny<RenderedBlock>()), Times.Once);
        mock.VerifyNoOtherCalls();        
    }
    
    [Fact]
    public void Render_DocumentWithMultipleElements_CorrectInvocations()
    {
        // Arrange
        var mock = new Mock<IRenderer>();
        var document = SharpTeX.TeXBlock.Document.Document.CreateDocument();
        var listBlock = SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize()
            .AddItem(TextBlock.CreateTextBlock("TextBlock1"));
        document.AddTitle();
        document.AddBlock(listBlock);
        document.AddBlock(TextBlock.CreateTextBlock("TextBlock2"));
        
        // Act
        var result = document.Render(mock.Object);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        mock.Verify(x => x.AddNamedBlock(document.BlockName, null), Times.Once);
        mock.Verify(x => x.AddToBlock(result.Value, @"\maketitle"), Times.Once);
        mock.Verify(x => x.AddNamedBlock(listBlock.BlockName, null), Times.Once);
        mock.Verify(x => x.AddToBlock(result.Value, "\\item TextBlock1"), Times.Once);
        mock.Verify(x => x.AddToBlock(It.IsAny<RenderedBlock>(), "TextBlock2"), Times.Once);
        mock.Verify(x => x.AddSimpleBlock(null), Times.Exactly(2));
        mock.Verify(x => x.AddToBlock(result.Value, It.IsAny<RenderedBlock>()), Times.Exactly(3));
        mock.VerifyNoOtherCalls();        
    }
    
    public static List<object[]> AddBlockTestCases =
    [
        new object[]
        {
            TextBlock.CreateTextBlock("TextBlock"),
            SharpTeX.TeXBlock.Document.Document.CreateDocument(),
            1,
            "Empty document, add one textblock"
        },
        new object[]
        {
            TextBlock.CreateTextBlock("TextBlock"),
            SharpTeX.TeXBlock.Document.Document.CreateDocument().AddBlock(
                SharpTeX.TeXBlock.ListBlock.ListBlock.NewItemize()
            ),
            2,
            "Document with a list, add textblock"
        },
    ];
}