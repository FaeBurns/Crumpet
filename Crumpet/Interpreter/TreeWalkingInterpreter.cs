using System.Diagnostics;
using Crumpet.Interpreter.Functions;
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
        Variable[] arguments = TransformArguments(args);
        UserFunction entryPoint = (UserFunction)context.FunctionResolver.GetFunction(entryPointName, arguments.Select(a => a.Type));

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
                TypeInfo type = new ArrayTypeInfo(GetTypeInfo(arg.GetType().GetElementType()!));
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
            TypeCode.String => BuiltinTypeInfo.String,
            TypeCode.Single => BuiltinTypeInfo.Float,
            
            // convert all to int
            TypeCode.Int64 => BuiltinTypeInfo.Int,
            TypeCode.Int32 => BuiltinTypeInfo.Int,
            TypeCode.Int16 => BuiltinTypeInfo.Int,
            TypeCode.UInt64 => BuiltinTypeInfo.Int,
            TypeCode.UInt32 => BuiltinTypeInfo.Int,
            TypeCode.UInt16 => BuiltinTypeInfo.Int,
            TypeCode.Byte => BuiltinTypeInfo.Int,
            TypeCode.SByte => BuiltinTypeInfo.Int,
            
            TypeCode.Boolean => BuiltinTypeInfo.Bool,
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