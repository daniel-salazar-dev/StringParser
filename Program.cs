using StringParser.Helpers;
using StringParser.Models;
using StringParser.Strategies;
using StringParser.Tests;

namespace StringParserChallenge;

/// <summary>
/// StringParser main program
/// </summary>
class Program
{
    static void Main()
    {
        string stringToParse = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";
        var menu = new Dictionary<string, string>()
        {
            {"1", "Running standard parsing strategies..."},
            {"2", "Running all parsing strategies..."},
            {"3", "Testing all parsing strategies..."},
            {"Q", "Exiting program..."},
        };

        Console.WriteLine("\nString Parser");
        Console.WriteLine("===================================");
        Console.WriteLine("\nPlease select from the following options:");
        Console.WriteLine("1: Run standard parsing strategies");
        Console.WriteLine("2: Run all parsing strategies");
        Console.WriteLine("3: Run tests");
        Console.WriteLine("Q: Quit program");

        string? userInput;
        do
        {
            string menuOptions = string.Join(", ", menu.Keys);

            Console.Write("\nEnter your choice: ");
            userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine($"Please enter a valid option ({menuOptions})");
                continue;
            }

            string _userInput = userInput.Trim().ToUpper();

            if (!menu.ContainsKey(_userInput))
            {
                Console.WriteLine($"Please enter a valid option ({menuOptions})");
                continue;
            }
        
            if (menu.TryGetValue(_userInput, out string? value))
            {
                Console.WriteLine($"\n{value}");
                if (_userInput == "1")
                    {
                        ProcessStrategies(stringToParse, false);
                    }
                else if (_userInput == "2")
                    {
                        ProcessStrategies(stringToParse, true);
                    }
                else if (_userInput == "3")
                    {
                        Strategies.TestAllTraversalStrategies(stringToParse);
                    }
                else if (_userInput == "Q")
                    {
                    return;
                    }
                continue;
            }

        } while (true);
    }

    static void ProcessStrategies(string input, bool includeAllStrategies = false)
    {
        try
        {
            Console.WriteLine($"Input: {input}");

            var tree = new FieldParser().Parse(input);

            Console.WriteLine($"\nString parsed successfully: Found {FieldNode.CountTotalNodes(tree)} total fields.");
            var results = new Dictionary<string, List<string>>();
            var registry = new TraversalStrategyRegistry();

            if (includeAllStrategies)
            {
                results = ExecuteTraversals(tree, registry, [.. registry.GetAvailableStrategies()]);
            }
            else
            {
                results = ExecuteTraversals(tree, new TraversalStrategyRegistry(), ["depth-first", "alphabetical"]);
            }

            OutputFormatter.PrintBulletList(results["depth-first"], "Output 1 (Depth-First Traversal)");
            OutputFormatter.PrintBulletList(results["alphabetical"], "Output 2 (Alphabetical Traversal)");
            if (includeAllStrategies)
            {
                OutputFormatter.PrintBulletList(results["breadth-first"], "Output 3 (Breadth-First Traversal)");
            }
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

    public static Dictionary<string, List<string>> ExecuteTraversals(FieldNode tree, TraversalStrategyRegistry registry, List<string> strategyNames)
    {
        var results = new Dictionary<string, List<string>>();

        foreach (var strategyName in strategyNames)
        {
            var strategy = registry.GetStrategy(strategyName);
            results[strategyName] = strategy.Traverse(tree);
        }

        return results;
    }
}
