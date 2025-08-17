namespace StringParser.Models;

public class FieldNode
{
    public string Name { get; }
    public List<FieldNode> Children { get; }

    public FieldNode(string name)
    {
        Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
        Children = new List<FieldNode>();
    }

    public void AddChild(FieldNode child)
    {
        Children.Add(child ?? throw new ArgumentNullException(nameof(child)));
    }

    public bool HasChildren => Children.Any();

    public override string ToString()
    {
        return HasChildren ? $"{Name}({Children.Count} children)" : Name;
    }

    public static int CountTotalNodes(FieldNode root)
    {
        int count = root.Name != "root" ? 1 : 0; // Don't count artificial root
        foreach (var child in root.Children)
        {
            count += CountTotalNodes(child);
        }
        return count;
    }
}