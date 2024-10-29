using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SharpTeX.Renderer.Enums;
using SharpTeX.Renderer.Models;

namespace SharpTeX.Renderer.Implementation;

public class Renderer : IRenderer
{
    private readonly Dictionary<string, RenderedBlock> _blocks = new();
    
    private RenderedBlock? _rootBlock;
    
    private readonly ILogger _logger;
    
    public Renderer(ILogger logger)
    {
        _logger = logger;
    }
    
    public void SetRootBlock(RenderedBlock block)
    {
        _rootBlock = block;
        _logger.LogInformation($"Renderer: Set root block with id '{block.BlockId}'.");
    }

    public RenderedBlock AddNamedBlock(string name, string? content = null)
    {
        var block = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(), 
            BlockName: name, 
            Content: content ?? string.Empty, 
            BlockType: BlockType.Named,
            Children: []
        );
        _blocks.Add(block.BlockId, block);
        _logger.LogInformation($"Renderer: Added named block '{name}' with id '{block.BlockId}'.");
        
        UpdateRootBlock(block);
        
        return block;
    }

    public RenderedBlock AddSimpleBlock(string? content = null)
    {
        var block = new RenderedBlock
        (
            BlockId: Guid.NewGuid().ToString(), 
            BlockName: string.Empty, 
            Content: content ?? string.Empty, 
            BlockType: BlockType.Simple,
            Children: []
        );        
        _blocks.Add(block.BlockId, block);
        _logger.LogInformation($"Renderer: Added simple block with id '{block.BlockId}'.");
        
        UpdateRootBlock(block);
        
        return block;
    }

    public RenderedBlock AddToBlock(RenderedBlock block, string? content)
    {
        var newBlock = block with { Content = content ?? string.Empty };
        _blocks[block.BlockId] = newBlock;
        _logger.LogInformation($"Renderer: Added content to block with id '{block.BlockId}'.");
        
        UpdateRootBlock(newBlock);
        
        return newBlock;
    }

    public RenderedBlock AddToBlock(RenderedBlock block, RenderedBlock content)
    {
        var  newBlock = block with { Children = block.Children.Append(content).ToList() };
        _blocks[block.BlockId] = newBlock;
        _logger.LogInformation($"Renderer: Added content to block with id '{block.BlockId}'.");
        
        UpdateRootBlock(newBlock);
        
        return newBlock;
    }

    public Result<string> Render()
    {
        if (_rootBlock is null)
        {
            _logger.LogError("Renderer: Root block is not set. Rendering failed.");
            return Result.Failure<string>("Root block is not set. Rendering failed.");
        }
        
        _logger.LogInformation("Renderer: Starting rendering.");
        var render = Result.Success(RenderBlock(_rootBlock));
        _logger.LogInformation("Renderer: Finished rendering.");
        
        return render;
    }
    
    public void LogFailure(string message)
    {
        _logger.LogError(message);
    }
    
    private string RenderNamedBlock(RenderedBlock block)
    {
        var contentBuilder = new StringBuilder();
        
        _logger.LogInformation($"Renderer: Rendering block '{block.BlockName}' with id '{block.BlockId}'.");
        contentBuilder.AppendLine($"\\begin{{{block.BlockName}}}");
        contentBuilder.AppendLine(block.Content);
        foreach (var childBlock in block.Children)
        {
            contentBuilder.AppendLine(RenderBlock(childBlock));
            _logger.LogInformation($"Renderer: Block: '{block.BlockId}' Finished rendering child with id '{childBlock.BlockId}'");
        }
        contentBuilder.AppendLine($"\\end{{{block.BlockName}}}");
        _logger.LogInformation($"Renderer: Finished rendering block '{block.BlockName}' with id '{block.BlockId}'.");
        
        return contentBuilder.ToString();
    }
    
    private string RenderSimpleBlock(RenderedBlock block)
    {
        _logger.LogInformation($"Renderer: Rendering simple block with id '{block.BlockId}'.");
        var render = block.Content;
        _logger.LogInformation($"Renderer: Finished rendering simple block with id '{block.BlockId}'.");
        
        return render;
    }
    
    private string RenderBlock(RenderedBlock block)
    {
        return block.BlockType switch
        {
            BlockType.Named => RenderNamedBlock(block),
            BlockType.Simple => RenderSimpleBlock(block),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private void UpdateRootBlock(RenderedBlock block)
    {
        if(_rootBlock is null)
        {
            SetRootBlock(block);
            return;
        }

        if (!block.BlockId.Equals(_rootBlock.BlockId))
        {
            return;
        }
        
        _logger.LogInformation($"Renderer: Updating root block with id '{block.BlockId}'.");
        SetRootBlock(block);
    }
}