using StringParser.Models;

namespace StringParser.Helpers;

/// <summary>
/// Parses input string into tree structure
/// </summary>
public class FieldParser
{
    private string _input = string.Empty;
    private int _position;

    public FieldNode Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));

        _input = input.Trim();
        _position = 0;

        if (!ConsumeChar('('))
            throw new FormatException("Input must start with '('");

        var root = new FieldNode("root");
        ParseFieldList(root);

        if (!ConsumeChar(')'))
            throw new FormatException("Input must end with ')'");

        if (_position < _input.Length)
            throw new FormatException($"Unexpected characters after closing parenthesis: {_input.Substring(_position)}");

        return root;
    }

    private void ParseFieldList(FieldNode parent)
    {
        while (_position < _input.Length && _input[_position] != ')')
        {
            SkipWhitespace();
            
            if (_position >= _input.Length || _input[_position] == ')')
                break;

            var field = ParseField();
            parent.AddChild(field);

            SkipWhitespace();

            // Check for comma (optional for last item)
            if (_position < _input.Length && _input[_position] == ',')
            {
                _position++; // consume comma
                SkipWhitespace();
            }
        }
    }

    private FieldNode ParseField()
    {
        var fieldName = ReadFieldName();
        
        if (string.IsNullOrWhiteSpace(fieldName))
            throw new FormatException($"Expected field name at position {_position}");

        var field = new FieldNode(fieldName);

        SkipWhitespace();

        // Check if this node has children (nested structure)
        if (_position < _input.Length && _input[_position] == '(')
        {
            _position++; // consume '('
            ParseFieldList(field);
            
            if (!ConsumeChar(')'))
                throw new FormatException($"Expected ')' after nested fields for '{fieldName}' at position {_position}");
        }

        return field;
    }

    private string ReadFieldName()
    {
        SkipWhitespace();
        
        var start = _position;
        
        // Read until we hit a delimiter or whitespace
        while (_position < _input.Length && 
                _input[_position] != ',' && 
                _input[_position] != '(' && 
                _input[_position] != ')' &&
                !char.IsWhiteSpace(_input[_position]))
        {
            _position++;
        }

        return _input.Substring(start, _position - start).Trim();
    }

    private bool ConsumeChar(char expected)
    {
        SkipWhitespace();
        
        if (_position < _input.Length && _input[_position] == expected)
        {
            _position++;
            return true;
        }
        
        return false;
    }

    private void SkipWhitespace()
    {
        while (_position < _input.Length && char.IsWhiteSpace(_input[_position]))
        {
            _position++;
        }
    }
}