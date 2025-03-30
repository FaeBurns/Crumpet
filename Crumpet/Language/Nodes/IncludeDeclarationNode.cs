using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class IncludeDeclarationNode : NonTerminalNode, INonTerminalNodeFactory
{
    public string Target { get; }
    
    public IncludeDeclarationNode(StringLiteralNode targetFile) : base(targetFile)
    {
        Target = targetFile.StringLiteral;

        if (Path.GetExtension(Target) == String.Empty)
        {
            // add if no extension was given
            Target += ".crm";
        }
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<IncludeDeclarationNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.PREPROCESSOR),
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_INCLUDE),
                new CrumpetTerminalConstraint(CrumpetToken.STRING)
                ), 
            GetNodeConstructor<IncludeDeclarationNode>());
    }
}