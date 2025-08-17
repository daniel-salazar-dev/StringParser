using StringParser.Models;

namespace StringParser.Strategies;

/// <summary>
/// Interface for traversal strategies
/// </summary>
public interface ITraversalStrategy
{
    string Name { get; }
    List<string> Traverse(FieldNode root);
}