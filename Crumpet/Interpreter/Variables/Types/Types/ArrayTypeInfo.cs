using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Variables.Types;

public class ArrayTypeInfo : ArrayTypeInfoUnkownTypeInfo
{
    public static readonly TypeInfo Any = new ArrayTypeInfoUnkownTypeInfo();
    
    public ArrayTypeInfo(TypeInfo innerType)
    {
        InnerType = innerType;
    }

    public TypeInfo InnerType { get; }
    public override string TypeName => InnerType.TypeName + "[]";

    protected override bool Equals(TypeInfo other)
    {
        if (other is ArrayTypeInfo arrayOther)
        {
            return base.Equals(other) && arrayOther.InnerType == InnerType;
        }

        return false;
    }

    public override Variable CreateVariable()
    {
        return Variable.Create(this, new List<Variable>());
    }

    public override object CreateCopy(object? instance)
    {
        if (instance is IList<Variable> source)
        {
            // create a copy of each element in the source
            return source.Select(Variable.CreateCopy).ToList();
        }

        throw new ArgumentException(ExceptionConstants.INVALID_TYPE.Format(typeof(IList<Variable>), instance!.GetType()));
    }

    public override bool ConvertableTo(TypeInfo other)
    {
        if (other is ArrayTypeInfoUnkownTypeInfo)
            return true;

        if (other is not ArrayTypeInfo arrayType)
            return false;

        if (InnerType.ConvertableTo(arrayType.InnerType))
            return true;
        
        return base.ConvertableTo(other);
    }

    public override object? ConvertValidObjectTo(TypeInfo type, object? value)
    {
        if (value is null)
            throw new NullReferenceException();
        
        if (type is ArrayTypeInfoUnkownTypeInfo)
            return value;

        if (type is ArrayTypeInfo arrayType)
        {
            List<Variable> result = new List<Variable>();
            foreach (Variable source in (value as List<Variable>)!)
            {
                Variable var = Variable.Create(arrayType.InnerType, source.Type.ConvertValidObjectTo(arrayType.InnerType, source.DereferenceToValue()));
                result.Add(var);
            }

            return result;
        }
        
        return base.ConvertValidObjectTo(type, value);
    }

    public void AddSlot(Variable array)
    {
        List<Variable> list = array.GetValue<List<Variable>>();
        Variable element = Variable.Create(InnerType);
        list.Add(element);
    }
    
    public override int GetObjectHashCode(Variable variable)
    {
        variable = variable.DereferenceToLowestVariable();
        List<Variable> array = variable.GetValue<List<Variable>>();
        
        // use default hash code
        return array.GetHashCode();
    }
}

public class ArrayTypeInfoUnkownTypeInfo : TypeInfo
{
    public override string TypeName => "[]";
    public override Variable CreateVariable()
    {
        throw new InvalidOperationException();
    }

    public override object CreateCopy(object? instance)
    {
        throw new InvalidOperationException();
    }

    public override int GetObjectHashCode(Variable variable)
    {
        throw new InvalidOperationException();
    }
}