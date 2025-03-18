using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes;
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
                node.Parameters.Parameters.Select(
                    p => new ParameterDefinition(
                        p.Name.Terminal, 
                        m_typeResolver.ResolveType(p.Type.FullName) ?? throw new CompilationException(p, ExceptionConstants.UNKOWN_TYPE.Format(p.Type)),
                        p.VariableModifier)), 
                node.Location);
            
            functions.Add(BuildFunction(definition, node));
        }
        
        return new FunctionResolver(functions);
    }

    private Function BuildFunction(FunctionDefinition definition, FunctionDeclarationNode node)
    {
        IEnumerable instructions = node.StatementBody.GetInstructionsRecursive();
        InstructionCollator bodyInstructions = new InstructionCollator(instructions);

        return new Function(definition, bodyInstructions);
    }
}