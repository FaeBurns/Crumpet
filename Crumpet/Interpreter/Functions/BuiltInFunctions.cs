using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Functions;

public static class BuiltInFunctions
{
    public static IEnumerable<BuiltInFunction> GetFunctions()
    {
        yield return new BuiltInFunction("count", Count, ArrayTypeInfo.Any.Copy());
        yield return new BuiltInFunction("count", Count, ArrayTypeInfo.Any.Pointer());
        yield return new BuiltInFunction("count", Count, DictionaryTypeInfo.Any.Copy());
        yield return new BuiltInFunction("count", Count, DictionaryTypeInfo.Any.Pointer());
        
        // io
        yield return new BuiltInFunction("print", Print, new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("println", PrintLine, new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("input", Input);
        
        // lists
        yield return new BuiltInFunction("_list", ListConstructor, 1, new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("listAdd", ListAdd, new ArrayTypeInfoUnkownTypeInfo().Pointer(), new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("listInsert", ListInsert, new ArrayTypeInfoUnkownTypeInfo().Pointer(), new AnyTypeInfo().Copy(), BuiltinTypeInfo.Int.Copy());
        yield return new BuiltInFunction("listRemove", ListRemove, new ArrayTypeInfoUnkownTypeInfo().Pointer(), BuiltinTypeInfo.Int.Copy());
        
        // maps
        yield return new BuiltInFunction("_map", DictionaryConstructor, 2);
        yield return new BuiltInFunction("map_add", DictionaryAdd, new DictionaryTypeInfoUnknownType().Pointer(), new AnyTypeInfo().Copy(), new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("map_add", DictionaryAdd, new DictionaryTypeInfoUnknownType().Pointer(), new AnyTypeInfo().Copy(), new AnyTypeInfo().Pointer());
        yield return new BuiltInFunction("map_remove", DictionaryRemove, new DictionaryTypeInfoUnknownType().Pointer(), new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("map_has", DictionaryHas, new DictionaryTypeInfoUnknownType().Pointer(), new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("map_get", DictionaryGet, new DictionaryTypeInfoUnknownType().Pointer(), new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("map_set", DictionarySet, new DictionaryTypeInfoUnknownType().Pointer(), new AnyTypeInfo().Copy(), new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("map_set", DictionarySet, new DictionaryTypeInfoUnknownType().Pointer(), new AnyTypeInfo().Copy(), new AnyTypeInfo().Pointer());
        
        // control
        yield return new BuiltInFunction("assert", AssertMessage, BuiltinTypeInfo.Bool.Copy(), BuiltinTypeInfo.String.Copy());
        yield return new BuiltInFunction("assert", Assert, BuiltinTypeInfo.Bool.Copy());
        yield return new BuiltInFunction("exit", Exit, BuiltinTypeInfo.Int.Copy());
        yield return new BuiltInFunction("throw", Throw, BuiltinTypeInfo.String.Copy());
        
        // strings
        yield return new BuiltInFunction("pInt", ParseInt, BuiltinTypeInfo.String.Copy());
        yield return new BuiltInFunction("pFloat", ParseFloat, BuiltinTypeInfo.String.Copy());
        yield return new BuiltInFunction("pString", ToString, new AnyTypeInfo().Copy());
        yield return new BuiltInFunction("toLower", StringToLower, BuiltinTypeInfo.String.Copy());
        yield return new BuiltInFunction("toUpper", StringToUpper, BuiltinTypeInfo.String.Copy());
        
        // # just debug things
        yield return new BuiltInFunction("BREAK", (_) => Debugger.Break());
        yield return new BuiltInFunction("ASSERT_STACK_COUNT", AssertStackCount, BuiltinTypeInfo.Int.Copy());
    }

    private static ParameterInfo Copy(this TypeInfo typeInfo) => new ParameterInfo(typeInfo, VariableModifier.COPY);
    private static ParameterInfo Pointer(this TypeInfo typeInfo) => new ParameterInfo(typeInfo, VariableModifier.POINTER);
    
    public static void Count(InterpreterExecutionContext context)
    {
        // dereference or self
        Variable value = context.VariableStack.Pop().DereferenceToLowestVariable();

        if (value.GetValue() is IEnumerable enumerable)
        {
            // IEnumerable does not have a count method
            int count = 0;
            foreach (object _ in enumerable) count++;
            
            context.VariableStack.Push(BuiltinTypeInfo.Int, count);
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
        context.VariableStack.Push(BuiltinTypeInfo.String, input);
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
            // throw new RuntimeException(RuntimeExceptionNames.ASSERT, $"{context.LastEqualityComparedVariables[1].GetValue()} != {context.LastEqualityComparedVariables[0].GetValue()}");
        {
            // kinda hate this
            if (context.CurrentUnit!.Unit.Instructions[context.CurrentUnit!.InstructionPointer - 1] is ExecuteFunctionInstruction executeInstruction)
            {
                using StreamReader sr = File.OpenText(executeInstruction.Location.SourceFileName);
                TextSliceReader textSliceReader = new TextSliceReader(sr.BaseStream);
                ReadOnlySpan<char> sourceSpan = textSliceReader.Read(executeInstruction.Location.StartOffset, executeInstruction.Location.EndOffset);

                throw new RuntimeException(RuntimeExceptionNames.ASSERT, $"{sourceSpan}");
            }

            // should technically be unreachable but I don't like it potentially going without throwing
            Debug.Assert(false);
            throw new UnreachableException();
        }
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
        Variable type = context.VariableStack.Pop();
        Variable count = context.VariableStack.Pop();

        TypeInfo typeArg = type.GetValue<TypeInfo>();
        ArrayTypeInfo arrayType = new ArrayTypeInfo(typeArg);
        Variable result = Variable.Create(arrayType);

        for (int i = 0; i < count.GetValue<int>(); i++)
        {
            arrayType.AddSlot(result);
        }
        
        context.VariableStack.Push(result);
    }

    public static void ListAdd(InterpreterExecutionContext context)
    {
        Variable value = context.VariableStack.Pop();
        Variable list = context.VariableStack.Pop().Dereference();

        if (list.Type is ArrayTypeInfo type)
        {          
            // add a copy of value to the end
            List<Variable> target = list.GetValue<List<Variable>>();
            target.Add(value.CreateCopyConvert(type.InnerType));
            return;
        }

        throw new ArgumentException(ExceptionConstants.INVALID_TYPE.Format(typeof(ArrayTypeInfo), list.Type));
    }

    public static void ListInsert(InterpreterExecutionContext context)
    {
        Variable value = context.VariableStack.Pop();
        Variable index = context.VariableStack.Pop();
        Variable list = context.VariableStack.Pop().Dereference();

        if (list.Type is ArrayTypeInfo type)
        {
            // add a copy of value to the end
            List<Variable> target = list.GetValue<List<Variable>>();
            target.Insert(index.GetValue<int>(), value.CreateCopyConvert(type.InnerType));
            return;
        }

        throw new ArgumentException(ExceptionConstants.INVALID_TYPE.Format(typeof(ArrayTypeInfo), list.Type));
    }

    public static void ListRemove(InterpreterExecutionContext context)
    {
        Variable index = context.VariableStack.Pop();
        Variable list = context.VariableStack.Pop().Dereference();

        if (list.Type is ArrayTypeInfo)
        {
            // add a copy of value to the end
            List<Variable> target = list.GetValue<List<Variable>>();
            target.RemoveAt(index.GetValue<int>());
            return;
        }

        throw new ArgumentException(ExceptionConstants.INVALID_TYPE.Format(typeof(ArrayTypeInfo), list.Type));
    }

    public static void Throw(InterpreterExecutionContext context)
    {
        Variable message = context.VariableStack.Pop();
        context.Throw(message.GetValue<string>());
    }

    public static void StringToLower(InterpreterExecutionContext context)
    {
        Variable str = context.VariableStack.Pop();
        string lower = str.GetValue<string>().ToLower();
        context.VariableStack.Push(BuiltinTypeInfo.String, lower);
    }

    public static void StringToUpper(InterpreterExecutionContext context)
    {
        Variable str = context.VariableStack.Pop();
        string lower = str.GetValue<string>().ToUpper();
        context.VariableStack.Push(BuiltinTypeInfo.String, lower);   
    }
    
    private static void AssertStackCount(InterpreterExecutionContext context)
    {
        int count = context.VariableStack.Pop().GetValue<int>();
        
        if (context.VariableStack.Count == count)
            return;

        Debugger.Break();
        throw new Exception();
    }

    private static void DictionaryConstructor(InterpreterExecutionContext context)
    {
        Variable valueType = context.VariableStack.Pop();
        Variable keyType = context.VariableStack.Pop();
        
        TypeInfo keyTypeArg = keyType.GetValue<TypeInfo>();
        TypeInfo valueTypeArg = valueType.GetValue<TypeInfo>();
        
        DictionaryTypeInfo dict = new DictionaryTypeInfo(keyTypeArg, valueTypeArg);
        Variable result = dict.CreateVariable();
        context.VariableStack.Push(result);
    }

    private static void DictionaryAdd(InterpreterExecutionContext context)
    {
        Variable varToAdd = context.VariableStack.Pop();
        Variable key = context.VariableStack.Pop();
        Variable targetDict = context.VariableStack.Pop().Dereference();

        IDictionary<int,Variable> target = targetDict.GetValue<IDictionary<int, Variable>>();
        int keyHash = key.GetObjectHashCode();
        target.Add(keyHash, Variable.CreateCopy(varToAdd));
    }

    private static void DictionaryRemove(InterpreterExecutionContext context)
    {
        Variable key = context.VariableStack.Pop();
        Variable targetDict = context.VariableStack.Pop().Dereference();
        
        IDictionary<int,Variable> target = targetDict.GetValue<IDictionary<int, Variable>>();
        int keyHash = key.GetObjectHashCode();
        bool removeResult = target.Remove(keyHash);
        
        context.VariableStack.Push(BuiltinTypeInfo.Bool, removeResult);
    }

    private static void DictionaryHas(InterpreterExecutionContext context)
    {
        Variable key = context.VariableStack.Pop();
        Variable targetDict = context.VariableStack.Pop().Dereference();
        
        IDictionary<int,Variable> target = targetDict.GetValue<IDictionary<int, Variable>>();
        int keyHash = key.GetObjectHashCode();
        bool hasResult = target.ContainsKey(keyHash);
        
        context.VariableStack.Push(BuiltinTypeInfo.Bool, hasResult);
    }

    private static void DictionaryGet(InterpreterExecutionContext context)
    {
        Variable key = context.VariableStack.Pop();
        Variable targetDict = context.VariableStack.Pop().Dereference();
        
        IDictionary<int,Variable> target = targetDict.GetValue<IDictionary<int, Variable>>();
        int keyHash = key.GetObjectHashCode();
        Variable result = target[keyHash];
        context.VariableStack.Push(result);
    }

    private static void DictionarySet(InterpreterExecutionContext context)
    {
        Variable varToAdd = context.VariableStack.Pop();
        Variable key = context.VariableStack.Pop();
        Variable targetDict = context.VariableStack.Pop().Dereference();

        IDictionary<int,Variable> target = targetDict.GetValue<IDictionary<int, Variable>>();
        int keyHash = key.GetObjectHashCode();
        target[keyHash] = Variable.CreateCopy(varToAdd);
    }
}
