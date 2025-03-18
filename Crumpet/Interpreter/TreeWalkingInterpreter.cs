using System.Diagnostics;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Preparse;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Crumpet.Language.Nodes;
using Parser;
using Parser.Nodes;
using Shared;

namespace Crumpet.Interpreter;

public class TreeWalkingInterpreter
{
    public FunctionResolver FunctionResolver { get; }
    public TypeResolver TypeResolver { get; }

    public TreeWalkingInterpreter(NonTerminalNode root)
    {
        ASTNode[] nodeSequence = NodeSequenceEnumerator.CreateSequential(root).ToArray();
        TypeResolver = new TypeBuilder(nodeSequence).GetTypeDefinitions();
        FunctionResolver = new FunctionBuilder(nodeSequence.OfType<FunctionDeclarationNode>(), TypeResolver).BuildFunctions();
    }

    public InterpreterExecutor Run(string entryPointName, params object[] args)
    {
        InterpreterExecutionContext context = new InterpreterExecutionContext(TypeResolver, FunctionResolver);

        // throws if fails to find
        Function entryPoint = context.FunctionResolver.GetFunction(entryPointName);
        Variable[] arguments = TransformArguments(args);

        // call immediately in context
        context.Call(entryPoint.CreateInvokableUnit(context, arguments));
        
        return new InterpreterExecutor(context);
    }

    private Variable[] TransformArguments(object[] args)
    {
        Variable[] arguments = new Variable[args.Length];
        for (int i = 0; i < args.Length; i++)
        {
            object arg = args[i];
            TypeInfo type = GetTypeInfo(arg.GetType());
            if (arg is IEnumerable<object> enumerable)
            {
                List<Variable> arrayElements = TransformArguments(enumerable.ToArray()).ToList();
                arguments[i] = Variable.Create(type, arrayElements);
            }

            if (type is DotnetObjectTypeInfo dnType)
                // don't like doing this manually but there is
                arguments[i] = Variable.Create(type, new DotnetObjectInstance(dnType, arg));
            else
                arguments[i] = Variable.Create(type, arg);
        }

        return arguments;
    }

    private TypeInfo GetTypeInfo(Type type)
    {
        return Type.GetTypeCode(type) switch
        {
            TypeCode.String => new BuiltinTypeInfo<string>(),
            TypeCode.Single => new BuiltinTypeInfo<float>(),
            
            // convert all to int
            TypeCode.Int64 => new BuiltinTypeInfo<int>(),
            TypeCode.Int32 => new BuiltinTypeInfo<int>(),
            TypeCode.Int16 => new BuiltinTypeInfo<int>(),
            TypeCode.UInt64 => new BuiltinTypeInfo<int>(),
            TypeCode.UInt32 => new BuiltinTypeInfo<int>(),
            TypeCode.UInt16 => new BuiltinTypeInfo<int>(),
            TypeCode.Byte => new BuiltinTypeInfo<int>(),
            TypeCode.SByte => new BuiltinTypeInfo<int>(),
            
            TypeCode.Boolean => new BuiltinTypeInfo<bool>(),
            TypeCode.Object => new DotnetObjectTypeInfo(type),
            _ => throw new UnreachableException()
        };
    }
}

public class InterpreterExecutor
{
    private readonly InterpreterExecutionContext m_context;
    
    internal InterpreterExecutor(InterpreterExecutionContext context)
    {
        m_context = context;
    }

    public bool StepSingleInstruction()
    {
        if (m_context.CurrentUnit == null)
            return false;

        Instruction instruction = m_context.StepNextInstruction();
        instruction.Execute(m_context);

        return true;
    }

    public Variable StepUntilComplete()
    {
        // return null if invalid
        if (m_context.CurrentUnit == null)
            return null!;

        // execute all instructions
        while (StepSingleInstruction())
        {
        }

        return m_context.LatestReturnValue!;
    }
}