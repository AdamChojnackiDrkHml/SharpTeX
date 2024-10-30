using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SharpTeX.Logging;
using SharpTeX.Renderer.Enums;
using SharpTeX.Renderer.Models;
using SharpTeX.TeXBlock.Document;
using SharpTeX.TeXBlock.ListBlock;
using SharpTeX.TeXBlock.SimpleBlock.TextBlock;
using static SharpTeX.Test.Utilities.LoggerUtilities;

namespace SharpTeX.Test.Renderer;

public class RendererTests
{
    [Fact]
    public void AddNamedBlock_NameAndContentSet_NamedBlockIsAdded()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(logger.Object);
        var name = "Block";
        var content = "Content";
        var expectedBlock = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: name,
            Content: content,
            BlockType: BlockType.Named,
            Children: []
        );
        
        // Act
        var res = renderer.AddNamedBlock(name, content);

        // Assert
        res.Should().BeEquivalentTo(expectedBlock,
            options => options.Excluding(x => x.BlockId));
        VerifyLog(logger, LogLevel.Information, @$"Renderer: Added named block '{name}' with id '{res.BlockId}'.", Times.Once());
    }
    
    [Fact]
    public void AddSimpleBlock_ContentSet_SimpleBlockIsAdded()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(logger.Object);
        var content = "Content";
        var expectedBlock = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: string.Empty,
            Content: content,
            BlockType: BlockType.Simple,
            Children: []
        );
        
        // Act
        var res = renderer.AddSimpleBlock(content);

        // Assert
        res.Should().BeEquivalentTo(expectedBlock,
            options => options.Excluding(x => x.BlockId));
        VerifyLog(logger, LogLevel.Information, @$"Renderer: Added simple block with id '{res.BlockId}'.", Times.Once());
    }
    
    [Fact]
    public void AddToBlock_StringContent_ContentIsAddedToBlock()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(logger.Object);
        var block = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: "Block",
            Content: "Content",
            BlockType: BlockType.Named,
            Children: []
        );
        var content = "New content";
        var expectedBlock = block with { Content = content };
        
        // Act
        var res = renderer.AddToBlock(block, content);

        // Assert
        res.Should().BeEquivalentTo(expectedBlock);
        VerifyLog(logger, LogLevel.Information, $"Renderer: Added content to block with id '{block.BlockId}'.", Times.Once());
    }
    
    [Fact]
    public void AddToBlock_BlockContent_ContentIsAddedToBlock()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(logger.Object);
        var block = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: "Block",
            Content: "Content",
            BlockType: BlockType.Named,
            Children: []
        );
        var contentBlock = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: "Block",
            Content: "Content",
            BlockType: BlockType.Named,
            Children: []
        );
        var expectedBlock = block with { Children = [contentBlock]};
        
        // Act
        var res = renderer.AddToBlock(block, contentBlock);

        // Assert
        res.Should().BeEquivalentTo(expectedBlock);
        VerifyLog(logger, LogLevel.Information, $"Renderer: Added content to block with id '{block.BlockId}'.", Times.Once());
    }
    
    [Fact]
    public void SetRootBlock_BlockSet_RootBlockIsSet()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(logger.Object);
        var block = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: "Block",
            Content: "Content",
            BlockType: BlockType.Named,
            Children: []
        );
        
        // Act
        renderer.SetRootBlock(block);

        // Assert
        VerifyLog(logger, LogLevel.Information, $"Renderer: Set root block with id '{block.BlockId}'.", Times.Once());
    }
    
    [Fact]
    public void Render_NoBlocksSet_FailureLogged()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(logger.Object);
        
        // Act
        renderer.Render();

        // Assert
        VerifyLog(logger, LogLevel.Error,"Renderer: Root block is not set. Rendering failed.", Times.Once());
    }
    
    [Fact]
    public void Render_SingleChildrenlessBlock_RenderedContentReturned()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(logger.Object);
        var block = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: "Block",
            Content: "Content",
            BlockType: BlockType.Named,
            Children: []
        );
        renderer.SetRootBlock(block);
        var expectedContent = @"\begin{Block}" + Environment.NewLine
                                + "Content" + Environment.NewLine
                                + @"\end{Block}" + Environment.NewLine;
        
        // Act
        var res = renderer.Render();

        // Assert
        res.IsSuccess.Should().BeTrue();
        res.Value.Should().Be(expectedContent);
        VerifyLog(logger, LogLevel.Information, $"Renderer: Starting rendering.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Finished rendering.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Rendering block 'Block' with id '{block.BlockId}'.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Finished rendering block 'Block' with id '{block.BlockId}'.", Times.Once());
    }
    
    [Fact]
    public void Render_SingleBlockWithChildren_RenderedContentReturned()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(logger.Object);
        var block = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: "Block",
            Content: "Content",
            BlockType: BlockType.Named,
            Children: []
        );
        var childBlock = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(),
            BlockName: string.Empty,
            Content: "ChildContent",
            BlockType: BlockType.Simple,
            Children: []
        );
        renderer.SetRootBlock(block);
        renderer.AddToBlock(block, childBlock);
        var expectedContent = @"\begin{Block}" + Environment.NewLine
                                + "Content" + Environment.NewLine
                                + "ChildContent" + Environment.NewLine
                                + @"\end{Block}" + Environment.NewLine;
        
        // Act
        var res = renderer.Render();

        // Assert
        res.IsSuccess.Should().BeTrue();
        res.Value.Should().Be(expectedContent);
        VerifyLog(logger, LogLevel.Information, $"Renderer: Set root block with id '{block.BlockId}'.", Times.Exactly(2));
        VerifyLog(logger, LogLevel.Information, $"Renderer: Added content to block with id '{block.BlockId}'.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Updating root block with id '{block.BlockId}'.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Starting rendering.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Rendering block 'Block' with id '{block.BlockId}'.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Rendering simple block with id '{childBlock.BlockId}'.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Finished rendering simple block with id '{childBlock.BlockId}'.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Block: '{block.BlockId}' Finished rendering child with id '{childBlock.BlockId}'", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Finished rendering block 'Block' with id '{block.BlockId}'.", Times.Once());
        VerifyLog(logger, LogLevel.Information, $"Renderer: Finished rendering.", Times.Once());
        logger.VerifyNoOtherCalls();
    }

    [Fact]
    public void Render_RealTexProject_RenderedContentCorrect()
    {
        // Arrange
        var renderLogger = new Mock<ILogger>();
        var texLogger = new Mock<ILogger>();
        var renderer = new SharpTeX.Renderer.Implementation.Renderer(renderLogger.Object);
        var texProject = TeXProject.TeXProject.CreateTeXProject(
            title: "Test",
            logger: texLogger.Object,
            author: "Author"
        ).AddDocument(
            Document.CreateDocument()
                .AddTitle()
                .AddBlock(
                    ListBlock.NewItemize()
                        .AddItem("Item 1")
                        .AddItem("Item 2")
                )
                .AddBlock(
                    TextBlock.CreateTextBlock("Hello World!")
                )
        );
        var renderBlock = texProject.Render(renderer);
        var expectedRender = @"\documentclass{article}" + Environment.NewLine
                            + @"\usepackage[utf8]{inputenc}" + Environment.NewLine
                            + @"\usepackage[english]{babel}" + Environment.NewLine
                            + @"\usepackage[a4paper,top=2cm,bottom=2cm,left=3cm,right=3cm,marginparwidth=1.75cm]{geometry}" + Environment.NewLine
                            + @"\title{Test}" + Environment.NewLine
                            + @"\author{Author}" + Environment.NewLine
                            + @"\begin{document}" + Environment.NewLine
                            + @"\maketitle" + Environment.NewLine
                            + @"\begin{itemize}" + Environment.NewLine
                            + @"\item Item 1" + Environment.NewLine
                            + @"\item Item 2" + Environment.NewLine
                            + @"\end{itemize}" + Environment.NewLine
                            + "Hello World!" + Environment.NewLine
                            + @"\end{document}" + Environment.NewLine;
        
        
        // Act
        var res = renderer.Render();
        
        // Assert
        res.IsSuccess.Should().BeTrue();
        res.Value.Should().Be(expectedRender);
    }



}