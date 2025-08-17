using StringParser.Models;

namespace StringParser.Strategies;

/// <summary>
/// Breadth-first traversal strategy
/// Processes nodes at current level in logical order before going deeper
/// </summary>
public class BreadthFirstTraversalStrategy : ITraversalStrategy
{
    public string Name => "Breadth-First";

    public List<string> Traverse(FieldNode root)
    {
        var result = new List<string>();
        var currentLevel = new List<(FieldNode node, int depth)>();
        
        // Start with all root children at depth 0
        foreach (var child in root.Children)
        {
            currentLevel.Add((child, 0));
        }
        
        // Process each level completely before moving to the next
        while (currentLevel.Count > 0)
        {
            var nextLevel = new List<(FieldNode node, int depth)>();
            
            // Process all leaf nodes (no children) at current level
            foreach (var (node, depth) in currentLevel.Where(n => n.node.Children.Count == 0))
            {
                string indentation = new string(' ', depth * 2);
                result.Add($"{indentation}- {node.Name}");
            }
            
            // Process all parent nodes (with children) at current level
            foreach (var (node, depth) in currentLevel.Where(n => n.node.Children.Count > 0))
            {
                string indentation = new string(' ', depth * 2);
                result.Add($"{indentation}- {node.Name}");
                
                // Collect children for next level
                foreach (var child in node.Children)
                {
                    nextLevel.Add((child, depth + 1));
                }
            }
            
            // Move to next level
            currentLevel = nextLevel;
        }
        
        return result;
    }
}