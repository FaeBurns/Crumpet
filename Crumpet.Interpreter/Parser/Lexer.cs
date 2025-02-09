using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Crumpet.Interpreter.Exceptions;

namespace Crumpet.Interpreter.Parser;

public class Lexer<T> : ILexer<T> where T : Enum
{
    private readonly List<TokenRule<T>> m_rules = new List<TokenRule<T>>();
    private readonly string m_source;

    private int m_head = 0;
    private int m_lineNumber = 0;
    private int m_columnNumber = 0;
    
    public Lexer(string source)
    {
        m_source = source;
        
        foreach (TokenRule<T> rule in GetTokenRules())
        {
            m_rules.Add(rule);
        }
    }
    
    public IEnumerator<Token<T>> Tokenize()
    {
        while(m_head != m_source.Length - 1)
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
            yield return result.Token;
        }
    }

    private TokenSearchResult? FindNextToken(ReadOnlySpan<char> searchArea)
    {
        int searchDepth = 1;
        while (searchDepth <= searchArea.Length)
        {
            foreach (TokenRule<T> rule in m_rules)
            {
                ReadOnlySpan<char> seekArea = searchArea.Slice(0, searchDepth);
                
                // have to use the string version as the span version does not return Match
                string matchString = seekArea.ToString();
                Match match = rule.Regex.Match(matchString);
                
                // if the match was a success on the entirety of the seeking area
                if (match.Success && match.Index == 0 && match.Length == searchDepth)
                {
                    return new TokenSearchResult(
                        new Token<T>(
                            rule.TokenId, 
                            matchString,
                            m_lineNumber,
                            new Range(
                                m_columnNumber, 
                                m_columnNumber + searchDepth
                            )),
                        rule,
                        searchDepth
                    );
                }
            }

            // search deeper
            searchDepth++;
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
            TokenAttribute tokenAttribute = GetEnumAttribute<TokenAttribute>(tokenValue);
            
            Regex regex = new Regex(tokenAttribute.Regex, RegexOptions.Compiled);
            rules.Add(new TokenRule<T>(tokenValue, regex, tokenAttribute));
        }

        return rules;
    }
    
    private static TAttribute GetEnumAttribute<TAttribute>(Enum enumVal) where TAttribute : Attribute
    {
        Type type = enumVal.GetType();
        MemberInfo[] memberInfos = type.GetMember(Enum.GetName(enumVal.GetType(), enumVal)!);
        return memberInfos[0].GetCustomAttribute<TAttribute>()!;
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