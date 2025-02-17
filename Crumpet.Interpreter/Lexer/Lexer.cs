using System.Reflection;
using System.Text.RegularExpressions;
using Crumpet.Interpreter.Exceptions;

namespace Crumpet.Interpreter.Lexer;

public class Lexer<T> : ILexer<T> where T : Enum
{
    private readonly List<TokenRule<T>> m_rules = new List<TokenRule<T>>();
    private readonly string m_source;
    private readonly HashSet<T> m_ignoreList;

    private int m_head = 0;
    private int m_lineNumber = 0;
    private int m_columnNumber = 0;
    
    public Lexer(string source, params IEnumerable<T> ignoreList)
    {
        m_source = source;
        m_ignoreList = new HashSet<T>(ignoreList);

        foreach (TokenRule<T> rule in GetTokenRules())
        {
            m_rules.Add(rule);
        }
    }
    
    public IEnumerable<Token<T>> Tokenize()
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
                throw new InvalidTokenException(m_lineNumber, m_columnNumber);
            }
            
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
            
            // return this token for the enumeration
            // but only if it's not in the ignore list
            // if it is then just go onto the next loop
            if (!m_ignoreList.Contains(result.Token.TokenId))
                yield return result.Token;
        }
    }

    private TokenSearchResult? FindNextToken(ReadOnlySpan<char> searchArea)
    {
        foreach (TokenRule<T> rule in m_rules)
        {
            bool found = false;
            int length = 0;
            // using EnumerateMatches allows for a ReadOnlySpan to be used
            foreach (ValueMatch match in rule.Regex.EnumerateMatches(searchArea))
            {
                if (match.Index == 0)
                {
                    length = match.Length;
                    found = true;
                    break;
                }
            }
                
            // if a match was found then return it
            // otherwise fail down and exit
            if (found)
            {
                string matchString = searchArea.Slice(0, length).ToString();
                
                return new TokenSearchResult(
                    new Token<T>(
                        rule.TokenId, 
                        matchString,
                        m_lineNumber,
                        new Range(
                            m_columnNumber, 
                            m_columnNumber + length
                        )),
                    rule,
                    length
                );
            }
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