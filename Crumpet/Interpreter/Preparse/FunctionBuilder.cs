using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
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
        List<Function> functions = new List<Function>();
        foreach (FunctionDeclarationNode node in m_functions)
        {
            // return function definition
            // types need to get their TypeInfo from the resolver and throw if they fail
            FunctionDefinition definition = new FunctionDefinition(
                node.Name.Terminal,
                m_typeResolver.ResolveType(node.ReturnType.FullName) ?? throw new CompilationException(node, ExceptionConstants.UNKOWN_RETURN_TYPE.Format(node.ReturnType)),
                node.ReturnModifier,
                node.Parameters.Parameters.Select(ParameterNodeToDefinition),
                node.Location);
            
            functions.Add(BuildFunction(definition, node));
        }
        
        // add built in functions
        functions.AddRange(BuiltInFunctions.GetFunctions());
        
        return new FunctionResolver(functions);
    }

    private ParameterDefinition ParameterNodeToDefinition(ParameterNode parameterNode)
    {
        switch (parameterNode)
        {
            case ParameterNodeBasicVariant basic:
                return new ParameterDefinition(
                    basic.Name.Terminal,
                    m_typeResolver.ResolveType(basic.Type.FullName) ?? throw new CompilationException(basic, ExceptionConstants.UNKOWN_TYPE.Format(basic.Type)),
                    basic.GetModifier(basic.ModifierSugar));
            case ParameterNodeArrayVariant array:
                return new ParameterDefinition(
                    array.Name.Terminal,
                    new ArrayTypeInfo(
                        m_typeResolver.ResolveType(array.Type.FullName)
                            ?? throw new CompilationException(array, ExceptionConstants.UNKOWN_TYPE.Format(array.Type))),
                    array.GetModifier(array.ModifierSugar));
            default:
                throw new UnreachableException();
        }
    }

    private Function BuildFunction(FunctionDefinition definition, FunctionDeclarationNode node)
    {
        IEnumerable instructions = node.GetInstructionsRecursive();
        InstructionCollator bodyInstructions = new InstructionCollator(instructions);

        return new UserFunction(definition, bodyInstructions);
    }
}