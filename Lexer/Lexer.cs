namespace Lexer;

public class Lexer
{
    private readonly string _input;
    private int _position = 0;

    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "if", TokenType.If }, { "else", TokenType.Else }, { "while", TokenType.While }, { "for", TokenType.For },
        { "move_up", TokenType.MoveUp }, { "move_down", TokenType.MoveDown },
        { "move_left", TokenType.MoveLeft }, { "move_right", TokenType.MoveRight },
        { "jump", TokenType.Jump }, { "attack", TokenType.Attack }, { "defend", TokenType.Defend },
        { "hero", TokenType.Hero }, { "enemy", TokenType.Enemy }, { "treasure", TokenType.Treasure },
        { "trap", TokenType.Trap }
    };

    public Lexer(string input)
    {
        _input = input;
    }

    private char CurrentChar => _position < _input.Length ? _input[_position] : '\0';

    private void Advance() => _position++;

    private void SkipWhitespace()
    {
        while (char.IsWhiteSpace(CurrentChar))
            Advance();
    }

    private void SkipComment()
    {
        if (CurrentChar == '/' && PeekNextChar() == '/')
        {
            while (CurrentChar != '\n' && CurrentChar != '\0')
                Advance();
        }
    }
    
    private char PeekNextChar()
    {
        return _position + 1 < _input.Length ? _input[_position + 1] : '\0';
    }
    
    private Token ReadNumber()
    {
        string num = "";
        while (char.IsDigit(CurrentChar))
        {
            num += CurrentChar;
            Advance();
        }

        return new Token(TokenType.Number, num);
    }

    private Token ReadIdentifier()
    {
        string identifier = "";
        while (char.IsLetterOrDigit(CurrentChar) || CurrentChar == '_')
        {
            identifier += CurrentChar;
            Advance();
        }

        if (Keywords.TryGetValue(identifier, out TokenType type))
            return new Token(type, identifier);
        return new Token(TokenType.Identifier, identifier);
    }

    public List<Token> Tokenize()
    {
        List<Token> tokens = new();

        while (_position < _input.Length)
        {
            SkipWhitespace();
            SkipComment();
            
            if (char.IsLetter(CurrentChar))
            {
                tokens.Add(ReadIdentifier());
                continue;
            }

            if (char.IsDigit(CurrentChar))
            {
                tokens.Add(ReadNumber());
                continue;
            }

            switch (CurrentChar)
            {
                case '+': tokens.Add(new Token(TokenType.Plus, "+")); break;
                case '-': tokens.Add(new Token(TokenType.Minus, "-")); break;
                case '(': tokens.Add(new Token(TokenType.Lparen, "(")); break;
                case ')': tokens.Add(new Token(TokenType.Rparen, ")")); break;
                case '{': tokens.Add(new Token(TokenType.Lbrace, "{")); break;
                case '}': tokens.Add(new Token(TokenType.Rbrace, "}")); break;
                case ';': tokens.Add(new Token(TokenType.Semicolon, ";")); break;
                case '\n': 
                case '\r': Advance(); continue;
                case '\0': break;
                default: throw new Exception($"Unexpected character: {CurrentChar}");
            }
          
            Advance();
        }

        tokens.Add(new Token(TokenType.Eof, "EOF"));
        return tokens;
    }
}