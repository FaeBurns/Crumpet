using System.Diagnostics;
using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Language;
using Crumpet.Interpreter.Parser;
using NUnit.Framework.Internal;

namespace Crumpet.Interpreter.Tests.Parser;

[TestFixture]
public class LexerTests
{
    private void TestTokenOutput(string input, params CrumpetToken[] tokens)
    {
        Lexer<CrumpetToken> lexer = new Lexer<CrumpetToken>(input);

        int i = 0;
        foreach (Token<CrumpetToken> token in lexer.Tokenize())
        {
            Assert.That(i, Is.LessThan(tokens.Length), $"Token {i} leaves expected range. Encountered {token.TokenId}");
            
            Assert.That(token.TokenId, Is.EqualTo(tokens[i]), $"Token {i}({token.TokenId}) did not match expected token {tokens[i]}");

            TestContext.Out.WriteLine($"Encountered token {i} {token.TokenId} as expected");
            i++;
        }
    }

    [Test]
    public void Test_InvalidToken()
    {
        // need at least one expected token
        Assert.Throws<InvalidTokenException>(() => TestTokenOutput("?", CrumpetToken.WHITESPACE));
    }

    [Test]
    public void Test_CommentIsOnlyTokenOnLine()
    {
        TestTokenOutput("// comment", CrumpetToken.COMMENT);
    }

    [Test]
    public void Test_CommentIsOnlyTokenMultiline()
    {
        TestTokenOutput("// comment\n", CrumpetToken.COMMENT, CrumpetToken.NEWLINE);
    }

    [Test]
    public void Test_NewlineTypes()
    {
        // adding extra whitespace for nicer formatting :3
        TestTokenOutput(" line\nline\r\n", CrumpetToken.WHITESPACE,
            CrumpetToken.IDENTIFIER, CrumpetToken.NEWLINE,
            CrumpetToken.IDENTIFIER, CrumpetToken.NEWLINE);
    }

    [Test]
    public void Test_Whitespace()
    {
        TestTokenOutput("test test test", CrumpetToken.IDENTIFIER, CrumpetToken.WHITESPACE, CrumpetToken.IDENTIFIER, CrumpetToken.WHITESPACE, CrumpetToken.IDENTIFIER);
    }

    [Test]
    public void Test_IgnoresToken()
    {
        CrumpetToken[] tokens = new[] { CrumpetToken.IDENTIFIER, CrumpetToken.IDENTIFIER, CrumpetToken.IDENTIFIER, CrumpetToken.IDENTIFIER };
        Lexer<CrumpetToken> lexer = new Lexer<CrumpetToken>("test test test\n   test", CrumpetToken.WHITESPACE, CrumpetToken.NEWLINE);

        int i = 0;
        foreach (Token<CrumpetToken> token in lexer.Tokenize())
        {
            Assert.That(i, Is.LessThan(tokens.Length), $"Token {i} leaves expected range. Encountered {token.TokenId}");
            
            Assert.That(token.TokenId, Is.EqualTo(tokens[i]), $"Token {i}({token.TokenId}) did not match expected token {tokens[i]}");

            TestContext.Out.WriteLine($"Encountered token {i} {token.TokenId} as expected");
            i++;
        }
    }
}