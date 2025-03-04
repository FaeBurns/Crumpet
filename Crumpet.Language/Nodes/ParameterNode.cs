using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public class ParameterNode : NonTerminalNode, INonTerminalNodeFactory
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken> SugarToken { get; }
    public IdentifierNode Name { get; }

    public ParameterNode(TypeNode type, TerminalNode<CrumpetToken>? sugarToken, IdentifierNode name) : base(type, sugarToken, name)
    {
        Type = type;
        SugarToken = sugarToken;
        Name = name;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ParameterNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.REFERENCE)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)), 
            GetNodeConstructor<ParameterNode>());
    }
}