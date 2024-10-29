using CSharpFunctionalExtensions;
using SharpTeX.Renderer.Models;

namespace SharpTeX.Renderer;

public interface IRenderer
{
    RenderedBlock AddNamedBlock(string name, string? content = null);
    
    RenderedBlock AddSimpleBlock(string? content = null);
    
    RenderedBlock AddToBlock(RenderedBlock block, string? content);
    
    RenderedBlock AddToBlock(RenderedBlock block, RenderedBlock content);
    
    Result<string> Render();
    
    void SetRootBlock(RenderedBlock block);
    
    void LogFailure(string message);
}