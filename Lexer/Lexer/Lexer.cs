﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using Shared;
using Shared.Exceptions;

namespace Lexer;

public class Lexer<T> : ILexer<T> where T : Enum
{
    private readonly List<TokenRule<T>> m_rules = new List<TokenRule<T>>();
    private readonly string m_source;
    private readonly string m_sourceFileName;
    private readonly HashSet<T> m_ignoreList;

    private int m_head = 0;
    private int m_lineNumber = 0;
    private int m_columnNumber = 0;

    public Lexer(string source, string sourceFileName, params IEnumerable<T> ignoreList)
    {
        m_source = source;
        m_sourceFileName = sourceFileName;
        m_ignoreList = new HashSet<T>(ignoreList);

        foreach (TokenRule<T> rule in GetTokenRules())
        {
            m_rules.Add(rule);
        }
    }

    public IEnumerable<Token<T>> Tokenize(bool includeComments = false)
    {
        // m_head gets the length of a section added - will equal m_source.Length when at the end
        while(m_head < m_source.Length)
        {
            // have to re-create on each loop due to yield
            // shouldn't negate effects of using span though
            ReadOnlySpan<char> source = m_source.AsSpan();
            TokenSearchResult? result = FindNextToken(source.Slice(m_head));

            // if no
            if (result == null)
            {
                #if DEBUG
                Debugger.Break();
                // run it again with debugger
                result = FindNextToken(source.Slice(m_head));
                #endif
                throw new InvalidTokenException(m_lineNumber, m_columnNumber);
            }

            result.Token.Location.SourceFileName = m_sourceFileName;
            result.Token.Location.StartLine = m_lineNumber;
            result.Token.Location.StartColumn = m_columnNumber;
            result.Token.Location.StartOffset = m_head;

            // check if rule is one that will result in a line break
            // anything with the newline flag should
            if (result.DetectedRule.Attribute.IsNewline)
            {
                // add to line number and reset column
                m_lineNumber++;
                m_columnNumber = 0;
            }
            else
            {
                // add to column number
                m_columnNumber += result.Length;
            }

            // increase
            m_head += result.Length;

            // set end of token location
            // column number should revert back to the end of the previous line if it's ending at the start of the current one
            result.Token.Location.EndLine = m_lineNumber;
            result.Token.Location.EndColumn = m_columnNumber == 0 ? (result.Token.Location.StartColumn + result.Length) : m_columnNumber;
            result.Token.Location.EndOffset = m_head;

            // return this token for the enumeration
            // but only if it's not in the ignore list
            // if it is then just go onto the next loop
            bool ignoreCheck = !m_ignoreList.Contains(result.Token.TokenId);
            bool commentCheck = includeComments || !result.DetectedRule.Attribute.IsComment;

            if (ignoreCheck && commentCheck)
                yield return result.Token;
        }
    }

    private TokenSearchResult? FindNextToken(ReadOnlySpan<char> searchArea)
    {
        bool found = false;
        int longest = 0;
        TokenRule<T> longestRule = null!;
        foreach (TokenRule<T> rule in m_rules)
        {
            // using EnumerateMatches allows for a ReadOnlySpan to be used
            foreach (ValueMatch match in rule.Regex.EnumerateMatches(searchArea))
            {
                if (match.Index == 0)
                {
                    if (match.Length > longest)
                    {
                        longest = match.Length;
                        longestRule = rule;
                        found = true;
                    }
                }
            }
        }
        
        // if a match was found then return it
        // otherwise fail down and exit
        if (found)
        {
            string matchString = searchArea.Slice(0, longest).ToString();

            return new TokenSearchResult(
                new Token<T>(
                    longestRule.TokenId,
                    matchString,
                    new SourceLocation()),
                longestRule,
                longest
            );
        }

        // failed to find any token, cry about it and return null
        return null;
    }

    /// <summary>
    /// Gets a collection of token rules.
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<TokenRule<T>> GetTokenRules()
    {
        List<TokenRule<T>> rules = new List<TokenRule<T>>();
        T[] orderedTokenTypes = ((T[])Enum.GetValues(typeof(T))).OrderBy(t => t).ToArray();

        foreach (T tokenValue in orderedTokenTypes)
        {
            TokenAttribute tokenAttribute = tokenValue.GetEnumAttribute<TokenAttribute>();

            Regex regex = new Regex(tokenAttribute.Regex, RegexOptions.Compiled);
            rules.Add(new TokenRule<T>(tokenValue, regex, tokenAttribute));
        }

        return rules;
    }

    private class TokenSearchResult
    {
        public TokenSearchResult(Token<T> token, TokenRule<T> detectedRule, int length)
        {
            Length = length;
            Token = token;
            DetectedRule = detectedRule;
        }

        public Token<T> Token { get; }
        public TokenRule<T> DetectedRule { get; }
        public int Length { get; }
    }
}