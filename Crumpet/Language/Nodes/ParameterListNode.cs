﻿using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class ParameterListNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ParameterNode[] Parameters { get; }

    public ParameterListNode(ParameterNode first, IEnumerable<ParameterNode> others)
    {
        Parameters = others.Prepend(first).ToArray();
    }

    public ParameterListNode()
    {
        Parameters = Array.Empty<ParameterNode>();
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ParameterListNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<ParameterNode>(),
                new ZeroOrMoreConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.COMMA),
                        new NonTerminalConstraint<ParameterNode>()))),
            GetNodeConstructor<ParameterListNode>());
    }

    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        foreach (ParameterNode node in Parameters)
        {
            yield return node;
        }
    }
}