using System.Text;

namespace StringParser.Helpers;

/// <summary>
/// Output formatting utilities
/// </summary>
public static class OutputFormatter
{
    public static void PrintBulletList(List<string> items, string title = "")
    {
        if (!string.IsNullOrEmpty(title))
        {
            Console.WriteLine($"\n{title}:");
            Console.WriteLine(new string('-', title.Length + 1));
        }

        foreach (var item in items)
        {
            Console.WriteLine(item); // Items already have "- " prefix and indentation
        }
    }
}