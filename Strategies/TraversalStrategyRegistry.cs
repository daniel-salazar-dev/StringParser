namespace StringParser.Strategies;

/// <summary>
/// Registry for managing traversal strategies
/// </summary>
public class TraversalStrategyRegistry
{
    private Dictionary<string, ITraversalStrategy> _strategies;

    public TraversalStrategyRegistry()
    {
        _strategies = new Dictionary<string, ITraversalStrategy>();
        RegisterTraversalStrategies();
    }

    private void RegisterTraversalStrategies()
    {
        Register(new DepthFirstTraversalStrategy());
        Register(new BreadthFirstTraversalStrategy());
        Register(new AlphabeticalTraversalStrategy());
    }

    public void Register(ITraversalStrategy strategy)
    {
        _strategies[strategy.Name.ToLower()] = strategy;
    }

    public ITraversalStrategy GetStrategy(string name)
    {
        return _strategies.TryGetValue(name.ToLower(), out var strategy) 
            ? strategy 
            : throw new ArgumentException($"Unknown traversal strategy: {name}");
    }

    public IEnumerable<string> GetAvailableStrategies()
    {
        return _strategies.Keys;
    }
}