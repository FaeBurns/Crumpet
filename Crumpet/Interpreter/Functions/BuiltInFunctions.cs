using Crumpet.Exceptions;
using Crumpet.Interpreter.Preparse;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Interpreter.Functions;

public static class BuiltInFunctions
{
    public static IEnumerable<BuiltInFunction> GetFunctions()
    {
        yield return new BuiltInFunction("count", Count);
        yield return new BuiltInFunction("print", Print);
        yield return new BuiltInFunction("println", PrintLine);
        yield return new BuiltInFunction("input", Input);
        yield return new BuiltInFunction("pInt", ParseInt);
        yield return new BuiltInFunction("pFloat", ParseFloat);
        yield return new BuiltInFunction("pString", ToString);
    }
    
    public static void Count(InterpreterExecutionContext context)
    {
        Variable value = context.VariableStack.Pop();

        if (value.Value is IEnumerable enumerable)
        {
            int count = 0;
            foreach (object _ in enumerable) count++;
            
            context.VariableStack.Push(Variable.Create(new BuiltinTypeInfo<int>(), count));
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
        context.VariableStack.Push(Variable.Create(new BuiltinTypeInfo<string>(), input));
    }

    public static void ParseInt(InterpreterExecutionContext context)
    {
        string input = context.VariableStack.Pop().GetValue<string>();
        if (Int32.TryParse(input, out int result))
            context.VariableStack.Push(new BuiltinTypeInfo<int>(), result);
        else
            throw new RuntimeException(RuntimeExceptionNames.TYPE);
    }
    
    public static void ParseFloat(InterpreterExecutionContext context)
    {
        string input = context.VariableStack.Pop().GetValue<string>();
        if (Single.TryParse(input, out float result))
            context.VariableStack.Push(new BuiltinTypeInfo<float>(), result);
        
        throw new RuntimeException(RuntimeExceptionNames.TYPE);
    }

    public static void ToString(InterpreterExecutionContext context)
    {
        Variable variable = context.VariableStack.Pop();
        context.VariableStack.Push(new BuiltinTypeInfo<string>(), variable.Value.ToString() ?? throw new RuntimeException(RuntimeExceptionNames.ARGUMENT));
    }
}