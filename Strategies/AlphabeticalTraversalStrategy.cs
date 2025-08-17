using StringParser.Models;

namespace StringParser.Strategies;

/// <summary>
/// Alphabetical depth-first traversal strategy
/// Processes nodes in alphabetical order
/// </summary>
public class AlphabeticalTraversalStrategy : ITraversalStrategy
{
    public string Name => "Alphabetical";

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

        var sortedChildren = node.Children.OrderBy(child => child.Name).ToList();
        foreach (var child in sortedChildren)
        {
            TraverseRecursive(child, result, skipRoot ? depth : depth + 1);
        }
    }
}