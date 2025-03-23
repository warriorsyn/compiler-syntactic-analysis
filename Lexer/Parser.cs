namespace Lexer;

using System;
using System.Collections.Generic;

public class Parser
{
    private List<Token> tokens;
    private int position = 0;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    private Token CurrentToken => position < tokens.Count ? tokens[position] : new Token(TokenType.Eof, "");

    private void Match(TokenType expected)
    {
        if (CurrentToken.Type == expected)
            position++;
        else
            throw new Exception($"Erro de sintaxe: esperado {expected}, encontrado {CurrentToken.Type}");
    }

    public void Parse()
    {
        while (CurrentToken.Type != TokenType.Eof)
        {
            ParseStatement();
        }
    }

    private void ParseStatement()
    {
        if (CurrentToken.Type == TokenType.If)
        {
            Match(TokenType.If);
            Match(TokenType.Lparen);
            ParseExpression();
            Match(TokenType.Rparen);
            Match(TokenType.Lbrace);

            if (CurrentToken.Type == TokenType.While || CurrentToken.Type == TokenType.For)
            {
                ProcessByTokenType(CurrentToken.Type);
            }
            else
            {
                ParseCommand();
            }

            Match(TokenType.Rbrace);
            if (CurrentToken.Type == TokenType.Else)
            {
                Match(TokenType.Else);
                Match(TokenType.Lbrace);
                if (CurrentToken.Type == TokenType.While || CurrentToken.Type == TokenType.For)
                {
                    ProcessByTokenType(CurrentToken.Type);
                }
                else
                {
                    ParseCommand();
                }

                Match(TokenType.Rbrace);
            }
        }
        else if (CurrentToken.Type == TokenType.While)
        {
            ProcessWhile();
        }
        else if (CurrentToken.Type == TokenType.For)
        {
            ProcessFor();
        }
    }

    private void ProcessByTokenType(TokenType tokenType)
    {
        if (tokenType == TokenType.For)
        {
            ProcessFor();
        }
        else if (tokenType == TokenType.While)
        {
            ProcessWhile();
        }
    }

    private void ProcessWhile()
    {
        Match(TokenType.While);
        Match(TokenType.Lparen);
        ParseExpression();
        Match(TokenType.Rparen);
        Match(TokenType.Lbrace);
        ParseCommand();
        Match(TokenType.Rbrace);
    }

    private void ProcessFor()
    {
        Match(TokenType.For);
        Match(TokenType.Lparen);
        ParseExpression();
        Match(TokenType.Semicolon);
        ParseExpression();
        Match(TokenType.Semicolon);
        ParseExpression();
        Match(TokenType.Rparen);
        Match(TokenType.Lbrace);
        ParseCommand();
        Match(TokenType.Rbrace);
    }

    private void ParseCommand()
    {
        if (CurrentToken.Type == TokenType.MoveUp || CurrentToken.Type == TokenType.MoveDown ||
            CurrentToken.Type == TokenType.MoveLeft || CurrentToken.Type == TokenType.MoveRight ||
            CurrentToken.Type == TokenType.Jump || CurrentToken.Type == TokenType.Attack ||
            CurrentToken.Type == TokenType.Defend)
        {
            position++;
        }
        else
        {
            throw new Exception($"Erro de sintaxe: comando inválido {CurrentToken.Value}");
        }
    }

    private void ParseExpression()
    {
        if (CurrentToken.Type == TokenType.Hero || CurrentToken.Type == TokenType.Enemy ||
            CurrentToken.Type == TokenType.Treasure || CurrentToken.Type == TokenType.Trap ||
            CurrentToken.Type == TokenType.Num)
        {
            position++;
            if (CurrentToken.Type == TokenType.Plus || CurrentToken.Type == TokenType.Minus)
            {
                position++;
                ParseExpression();
            }
        }
        else
        {
            throw new Exception($"Erro de sintaxe: expressão inválida {CurrentToken.Value}");
        }
    }
}