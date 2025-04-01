using Crumpet.Exceptions;
using Shared;

namespace Crumpet.Interpreter.Variables.Types;

public class DictionaryTypeInfo : DictionaryTypeInfoUnknownType
{
    public static readonly TypeInfo Any = new DictionaryTypeInfoUnknownType();
    public TypeInfo KeyType { get; }
    public TypeInfo ValueType { get; }

    public override string TypeName => "Map[" + KeyType.TypeName + "->" + ValueType.TypeName + "]";

    public DictionaryTypeInfo(TypeInfo keyType, TypeInfo valueType)
    {
        KeyType = keyType;
        ValueType = valueType;
    }
    
    protected override bool Equals(TypeInfo other)
    {
        if (other is DictionaryTypeInfo dictOther)
        {
            return base.Equals(other) && dictOther.KeyType == KeyType && dictOther.ValueType == ValueType;
        }

        return false;
    }

    public override bool ConvertableTo(TypeInfo other)
    {
        if (other is DictionaryTypeInfoUnknownType)
            return true;

        if (other is not DictionaryTypeInfo dictType)
            return false;

        if (KeyType != dictType.KeyType)
            return false;

        if (ValueType != dictType.ValueType)
            return false;
        
        return base.ConvertableTo(other);
    }

    public override object? ConvertValidObjectTo(TypeInfo type, object? value)
    {
        if (value is null)
            throw new NullReferenceException();
        
        if (type is DictionaryTypeInfoUnknownType)
            return value;
        
        return base.ConvertValidObjectTo(type, value);
    }

    public override int GetObjectHashCode(Variable variable)
    {
        variable = variable.DereferenceToLowestVariable();
        Dictionary<int, Variable> dict = variable.GetValue<Dictionary<int, Variable>>();
        
        // use default hash code
        return dict.GetHashCode();
    }
}

public class DictionaryTypeInfoUnknownType : TypeInfo
{
    public override string TypeName => "Map[?->?]";
    public override Variable CreateVariable()
    {
        return Variable.Create(this, new Dictionary<int, Variable>());
    }
    
    public override object CreateCopy(object? instance)
    {
        if (instance is IDictionary<int, Variable> source)
        {
            // create a copy of each element in the source
            return source.Select(CreateElementCopy).ToDictionary();
        }

        throw new ArgumentException(ExceptionConstants.INVALID_TYPE.Format(typeof(IDictionary<int, Variable>), instance!.GetType()));
    }
    
    private static KeyValuePair<int, Variable> CreateElementCopy(KeyValuePair<int, Variable> source) => new KeyValuePair<int, Variable>(source.Key, Variable.CreateCopy(source.Value));

    public override int GetObjectHashCode(Variable variable)
    {
        throw new InvalidOperationException();
    }
}