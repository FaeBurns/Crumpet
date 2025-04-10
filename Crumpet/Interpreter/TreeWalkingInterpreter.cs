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

    public InterpreterExecutor Run(string entryPointName, params string[] args)
    {
        InterpreterExecutionContext context = new InterpreterExecutionContext(TypeResolver, FunctionResolver, m_inputStream, m_outputStream);

        // throws if fails to find
        UserFunction entryPoint = (UserFunction)context.FunctionResolver.GetEntryPoint(entryPointName, args);
        Variable[] arguments;
        if (entryPoint.Parameters.Count == 1 && entryPoint.Parameters[0].Type is ArrayTypeInfo arrayType)
        {
            // ReSharper disable once CoVariantArrayConversion
            arguments = TransformArguments(args, Enumerable.Repeat(arrayType.InnerType, args.Length).ToArray());
            Variable arrayVar = arrayType.CreateVariable();
            arrayVar.GetValue<List<Variable>>().AddRange(arguments);
            arguments = [arrayVar];
        }
        else
            arguments = TransformArguments(args, entryPoint.Parameters.Select(p => p.Type).ToArray());

        // call immediately in context
        context.Call(entryPoint.CreateInvokableUnit(context, arguments));
        
        return new InterpreterExecutor(context);
    }

    private Variable[] TransformArguments(string[] args, TypeInfo[] types)
    {
        Variable[] arguments = new Variable[args.Length];
        for (int i = 0; i < args.Length; i++)
        {
            arguments[i] = Variable.Create(types[i], ConvertArg(args[i], types[i]));
        }

        return arguments;
    }

    private object ConvertArg(string arg, TypeInfo type)
    {
        switch (type)
        {
            case BuiltinTypeInfo<string>:
                return arg;
            case BuiltinTypeInfo<int>:
                return Int32.Parse(arg);
            case BuiltinTypeInfo<float>:
                return Single.Parse(arg);
            case BuiltinTypeInfo<bool>:
                return Boolean.Parse(arg);
            default:
                throw new Exception(ExceptionConstants.INVALID_ENTRY_POINT_ARGUMENT_TYPE.Format(type));
        }
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