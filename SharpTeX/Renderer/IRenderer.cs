using CSharpFunctionalExtensions;
using SharpTeX.Renderer.Models;

namespace SharpTeX.Renderer;

public interface IRenderer
{
    RenderedBlock AddNamedBlock(string name, string? content = null);
    
    RenderedBlock AddSimpleBlock(string? content = null);
    
    RenderedBlock AddToBlock(RenderedBlock block, string? content);
    
    RenderedBlock AddToBlock(RenderedBlock block, RenderedBlock content);
    
    RenderedBlock AddBlock(string content);
    
    Result<string> Render();
    
    void LogFailure(string message);
}