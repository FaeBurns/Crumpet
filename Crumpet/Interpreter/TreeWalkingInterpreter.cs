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
    private readonly Stream m_inputStream;
    private readonly Stream m_outputStream;
    public FunctionResolver FunctionResolver { get; }
    public TypeResolver TypeResolver { get; }

    public TreeWalkingInterpreter(NonTerminalNode root, Stream? inputStream = null, Stream? outputStream = null)
    {
        m_inputStream = inputStream ?? new EmptyInputStream();
        m_outputStream = outputStream ?? Stream.Null;
        ASTNode[] nodeSequence = NodeSequenceEnumerator.CreateSequential(root).ToArray();
        TypeResolver = new TypeBuilder(nodeSequence).GetTypeDefinitions();
        FunctionResolver = new FunctionBuilder(nodeSequence.OfType<FunctionDeclarationNode>(), TypeResolver).BuildFunctions();
    }

    public InterpreterExecutor Run(string entryPointName, params object[] args)
    {
        InterpreterExecutionContext context = new InterpreterExecutionContext(TypeResolver, FunctionResolver, m_inputStream, m_outputStream);

        // throws if fails to find
        UserFunction entryPoint = (UserFunction)context.FunctionResolver.GetFunction(entryPointName);
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
            if (arg is IEnumerable<object> enumerable)
            {
                TypeInfo type = new ArrayTypeInfo(GetTypeInfo(arg.GetType().GetElementType()!), VariableModifier.COPY);
                List<Variable> arrayElements = TransformArguments(enumerable.ToArray()).ToList();
                arguments[i] = Variable.Create(type, arrayElements);
            }
            else
            {
                TypeInfo type = GetTypeInfo(arg.GetType());
                arguments[i] = Variable.Create(type, arg);
            }
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
            _ => throw new UnreachableException()
        };
    }

    private sealed class EmptyInputStream : Stream
    {
        public override void Flush()
        {
            throw new InvalidOperationException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count < 1)
                throw new ArgumentException();
            
            buffer[offset] = (byte)'\n';
            return 1;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => 1;
        public override long Position { get; set; } = 0;
    }
}

public class InterpreterExecutor
{
    private readonly InterpreterExecutionContext m_context;
    
    internal InterpreterExecutor(InterpreterExecutionContext context)
    {
        m_context = context;
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

        // return last returned value or default of 0
        return m_context.LatestReturnValue ?? new BuiltinTypeInfo<int>().CreateVariable();
    }
    
    private bool StepSingleInstruction()
    {
        if (m_context.CurrentUnit == null)
            return false;

        // get the next instruction
        // null case means that there were no units left to get instructions from and the program is complete
        Instruction? instruction = m_context.StepNextInstruction();
        if (instruction == null)
            return false;
        
        instruction.Execute(m_context);

        return true;
    }
}