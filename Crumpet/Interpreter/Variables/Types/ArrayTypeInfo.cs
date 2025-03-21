using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Variables.Types;

public class ArrayTypeInfo : ArrayTypeInfoUnkownType
{
    public static readonly TypeInfo Any = new ArrayTypeInfoUnkownType(VariableModifier.COPY);
    
    public ArrayTypeInfo(TypeInfo innerType, VariableModifier innerTypeModifier) : base(innerTypeModifier)
    {
        InnerType = innerType;
    }

    public TypeInfo InnerType { get; }
    public override string TypeName => InnerType.TypeName + "[]";

    protected override bool Equals(TypeInfo other)
    {
        if (other is ArrayTypeInfo arrayOther)
        {
            return base.Equals(other) && arrayOther.InnerType == InnerType && arrayOther.InnerTypeModifier == InnerTypeModifier;
        }

        return false;
    }

    public override Variable CreateVariable()
    {
        return Variable.Create(this, new List<Variable>());
    }

    public override object CreateCopy(object instance)
    {
        if (instance is IList<Variable> source)
        {
            List<Variable> result = new List<Variable>(source.Count);
            foreach (Variable elem in source)
            {
                Variable copiedElementVariable = Variable.CreateModifier(elem.Type, InnerTypeModifier, elem);
                // do a manual assignment if it's a copy as CreateModifier won't set the value itself
                if (InnerTypeModifier == VariableModifier.COPY)
                    copiedElementVariable.Value = elem;
                result.Add(copiedElementVariable);
            }

            return result;
        }

        throw new ArgumentException(ExceptionConstants.INVALID_TYPE.Format(typeof(IList<Variable>), instance.GetType()));
    }
}

public class ArrayTypeInfoUnkownType : TypeInfo
{
    public VariableModifier InnerTypeModifier { get; }

    public ArrayTypeInfoUnkownType(VariableModifier innerTypeModifier)
    {
        InnerTypeModifier = innerTypeModifier;
    }
    
    public override string TypeName => "[]";
    public override Variable CreateVariable()
    {
        throw new InvalidOperationException();
    }

    public override object CreateCopy(object instance)
    {
        throw new InvalidOperationException();
    }
}