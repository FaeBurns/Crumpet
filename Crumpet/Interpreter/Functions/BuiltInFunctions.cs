using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

namespace Crumpet.Interpreter.Functions;

public static class BuiltInFunctions
{
    public static IEnumerable<BuiltInFunction> GetFunctions()
    {
        yield return new BuiltInFunction("count", Count, ArrayTypeInfo.Any.Copy());
        yield return new BuiltInFunction("print", Print, new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("println", PrintLine, new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("input", Input);
        yield return new BuiltInFunction("pInt", ParseInt, BuiltinTypeInfo.String.Copy());
        yield return new BuiltInFunction("pFloat", ParseFloat, BuiltinTypeInfo.String.Copy());
        yield return new BuiltInFunction("pString", ToString, new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("assert", Assert, BuiltinTypeInfo.Bool.Copy());
        yield return new BuiltInFunction("assert", AssertMessage, BuiltinTypeInfo.Bool.Copy(), BuiltinTypeInfo.String.Copy());
        yield return new BuiltInFunction("exit", Exit, BuiltinTypeInfo.Int.Copy());
        yield return new BuiltInFunction("list", ListConstructor, new TypeTypeInfoUnknownType().Copy(), new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("throw", Throw, BuiltinTypeInfo.String.Copy());
        yield return new BuiltInFunction("BREAK", (_) => Debugger.Break());
    }

    private static ParameterInfo Copy(this TypeInfo typeInfo) => new ParameterInfo(typeInfo, VariableModifier.COPY);
    private static ParameterInfo Pointer(this TypeInfo typeInfo) => new ParameterInfo(typeInfo, VariableModifier.POINTER);
    
    public static void Count(InterpreterExecutionContext context)
    {
        Variable value = context.VariableStack.Pop();

        if (value.GetValue() is IEnumerable enumerable)
        {
            int count = 0;
            foreach (object _ in enumerable) count++;
            
            context.VariableStack.Push(Variable.Create(BuiltinTypeInfo.Int, count));
            return;
        }

        throw new RuntimeException(RuntimeExceptionNames.ARGUMENT);
    }

    public static void Print(InterpreterExecutionContext context)
    {
        Variable value = context.VariableStack.Pop();
        context.WriteToOutputStream(value.GetValue<string>());
    }

    public static void PrintLine(InterpreterExecutionContext context)
    {
        Variable value = context.VariableStack.Pop();
        context.WriteToOutputStream(value.GetValue<string>() + "\n");
    }

    public static void Input(InterpreterExecutionContext context)
    {
        string input = context.ReadInputStreamLine(true);
        context.VariableStack.Push(Variable.Create(BuiltinTypeInfo.String, input));
    }

    public static void ParseInt(InterpreterExecutionContext context)
    {
        string input = context.VariableStack.Pop().GetValue<string>();
        if (Int32.TryParse(input, out int result))
            context.VariableStack.Push(BuiltinTypeInfo.Int, result);
        else
            throw new RuntimeException(RuntimeExceptionNames.TYPE);
    }
    
    public static void ParseFloat(InterpreterExecutionContext context)
    {
        string input = context.VariableStack.Pop().GetValue<string>();
        if (Single.TryParse(input, out float result))
            context.VariableStack.Push(BuiltinTypeInfo.Float, result);
        
        throw new RuntimeException(RuntimeExceptionNames.TYPE);
    }

    public static void ToString(InterpreterExecutionContext context)
    {
        Variable variable = context.VariableStack.Pop();
        context.VariableStack.Push(BuiltinTypeInfo.String, variable.GetValue()!.ToString() ?? throw new RuntimeException(RuntimeExceptionNames.ARGUMENT));
    }
    
    public static void Assert(InterpreterExecutionContext context)
    {
        Variable value = context.VariableStack.Pop();
        if (!value.GetValue<bool>())
            throw new RuntimeException(RuntimeExceptionNames.ASSERT, $"{context.LastEqualityComparedVariables[1].GetValue()} != {context.LastEqualityComparedVariables[0].GetValue()}");
    }

    public static void AssertMessage(InterpreterExecutionContext context)
    {
        string message = context.VariableStack.Pop().GetValue<string>();
        Variable value = context.VariableStack.Pop();
        if (!value.GetValue<bool>())
            throw new RuntimeException(RuntimeExceptionNames.ASSERT, message);
    }

    public static void Exit(InterpreterExecutionContext context)
    {
        Variable value = context.VariableStack.Pop();
        context.Exit(value.GetValue<int>());
    }

    public static void ListConstructor(InterpreterExecutionContext context)
    {
        Variable count = context.VariableStack.Pop();
        Variable type = context.VariableStack.Pop();

        TypeInfo typeArg = type.GetValue<TypeInfo>();
        ArrayTypeInfo arrayType = new ArrayTypeInfo(typeArg);
        Variable result = Variable.Create(arrayType);

        for (int i = 0; i < count.GetValue<int>(); i++)
        {
            arrayType.AddElement(result);
        }
        
        context.VariableStack.Push(result);
    }

    public static void ListAdd(InterpreterExecutionContext context)
    {
        
    }

    public static void Throw(InterpreterExecutionContext context)
    {
        Variable message = context.VariableStack.Pop();
        context.Throw(message.GetValue<string>());
    }
}