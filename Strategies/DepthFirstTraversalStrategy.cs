using StringParser.Models;

namespace StringParser.Strategies;

/// <summary>
/// Depth-first traversal strategy
/// Processes nodes in logical order
/// </summary>
public class DepthFirstTraversalStrategy : ITraversalStrategy
{
    public string Name => "Depth-First";

    public List<string> Traverse(FieldNode root)
    {
        var result = new List<string>();
        TraverseRecursive(root, result, 0, skipRoot: true);
        return result;
    }

    private void TraverseRecursive(FieldNode node, List<string> result, int depth, bool skipRoot = false)
    {
        if (!skipRoot)
        {
            string indentation = new string(' ', depth * 2);
            result.Add($"{indentation}- {node.Name}");
        }

        foreach (var child in node.Children)
        {
            TraverseRecursive(child, result, skipRoot ? depth : depth + 1);
        }
    }
}