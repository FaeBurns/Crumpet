using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class RootNonTerminalNode : NonTerminalNode, INonTerminalNodeFactory
{
    public DeclarationNode[] Declarations { get; }
    
    // ReSharper disable PossibleMultipleEnumeration
    // just gonna have to deal - it's likely it's an array anyway so it's fine
    private RootNonTerminalNode(IEnumerable<DeclarationNode> declarationNodes) : base(declarationNodes)
    {
        Declarations = declarationNodes.ToArray();
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("root_program",
            new ZeroOrMoreConstraint(new NonTerminalConstraint("declaration")), 
            GetNodeConstructor<RootNonTerminalNode>());
    }
}