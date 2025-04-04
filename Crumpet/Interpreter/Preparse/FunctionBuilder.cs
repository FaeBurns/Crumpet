using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Interpreter.Variables.Types.Templates;
using Crumpet.Language.Nodes;
using Crumpet.Language.Nodes.Terminals;
using Shared;

namespace Crumpet.Interpreter.Preparse;

public class FunctionBuilder
{
    private readonly IEnumerable<FunctionDeclarationNode> m_functions;
    private readonly TypeResolver m_typeResolver;

    public FunctionBuilder(IEnumerable<FunctionDeclarationNode> functions, TypeResolver typeResolver)
    {
        m_functions = functions;
        m_typeResolver = typeResolver;
    }
    
    public FunctionResolver BuildFunctions()
    {
        List<FunctionTemplate> userFunctions = new List<FunctionTemplate>();
        foreach (FunctionDeclarationNode node in m_functions)
        {
            HashSet<string> genericParamNames = node.TypeParams.GenericTypeNames.Select(t => t.Terminal).ToHashSet();
            
            // return function definition
            // types need to get their TypeInfo from the resolver and throw if they fail
            FunctionDefinition definition = new FunctionDefinition(
                node.Name.Terminal,
                // allow generic types as they'll be resolved in the execution later
                m_typeResolver.TypeNodeToTemplate(node.ReturnType),
                node.ReturnModifier,
                node.Parameters.Parameters.Select(p => ParameterNodeToDefinition(p, genericParamNames)),
                genericParamNames.ToArray(),
                node.Location);
            
            userFunctions.Add(BuildFunction(definition, node));
        }
        
        
        return new FunctionResolver(m_typeResolver, BuiltInFunctions.GetFunctions(), userFunctions);
    }

    private ParameterTemplate ParameterNodeToDefinition(ParameterNode node, HashSet<string> genericParamNames)
    {
        TypeTemplate type = m_typeResolver.TypeNodeToTemplate(node.Type);

        // contain in array if the param is an array variant
        if (node is ParameterNodeArrayVariant)
            type = new ArrayTypeTemplate(type);
        
        return new ParameterTemplate(node.Name.Terminal, type, node.GetModifier(node.ModifierSugar));
    }

    private FunctionTemplate BuildFunction(FunctionDefinition definition, FunctionDeclarationNode node)
    {
        IEnumerable instructions = node.GetInstructionsRecursive();
        InstructionCollator bodyInstructions = new InstructionCollator(instructions);

        return new UserFunctionTemplate(definition, bodyInstructions);
    }
}