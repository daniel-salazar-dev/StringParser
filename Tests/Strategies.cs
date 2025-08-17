using StringParser.Helpers;
using StringParser.Strategies;
using StringParserChallenge;

namespace StringParser.Tests;

/// <summary>
/// Validations for traversal strategies
/// </summary>
public class Strategies
{
    private static List<string> GetExpectedDepthFirst()
    {
        return new List<string> 
        { 
            "- id", 
            "- name", 
            "- email", 
            "- type", 
            "  - id", 
            "  - name", 
            "  - customFields", 
            "    - c1", 
            "    - c2", 
            "    - c3", 
            "- externalId" 
        };
    }

    private static List<string> GetExpectedAlphabetical()
    {
        return new List<string> 
        { 
            "- email", 
            "- externalId", 
            "- id", 
            "- name", 
            "- type", 
            "  - customFields", 
            "    - c1", 
            "    - c2", 
            "    - c3", 
            "  - id", 
            "  - name" 
        };
    }

    private static List<string> GetExpectedBreadthFirst()
    {
        return new List<string> 
        { 
            "- id", 
            "- name", 
            "- email", 
            "- externalId", 
            "- type", 
            "  - id", 
            "  - name" ,
            "  - customFields", 
            "    - c1", 
            "    - c2", 
            "    - c3", 
        };
    }

    private static void VerifyOutput(string traversalType, List<string> actual, List<string> expected)
    {
        bool matches = actual.SequenceEqual(expected);
        string status = matches ? "PASS" : "FAIL";
        
        Console.WriteLine($"{status} {traversalType}");
        
        if (!matches)
        {
            Console.WriteLine($"  Expected: [{string.Join(", ", expected)}]");
            Console.WriteLine($"  Actual:   [{string.Join(", ", actual)}]");
        }
    }
    public static void TestAllTraversalStrategies(string input)
    {
        try
        {
            var parser = new FieldParser();
            var tree = parser.Parse(input);
            var registry = new TraversalStrategyRegistry();

            var results = Program.ExecuteTraversals(tree, registry, ["depth-first", "alphabetical", "breadth-first"]);

            VerifyOutput("Depth-First", results["depth-first"], GetExpectedDepthFirst());
            VerifyOutput("Alphabetical", results["alphabetical"], GetExpectedAlphabetical());
            VerifyOutput("Breadth-First", results["breadth-first"], GetExpectedBreadthFirst());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (ex is FormatException)
            {
                Console.WriteLine("Please check that your input has proper parentheses and comma formatting.");
            }
        }

    }
}