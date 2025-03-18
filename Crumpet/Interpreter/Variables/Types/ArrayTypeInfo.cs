using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Variables.Types;

public class ArrayTypeInfo : TypeInfo
{
    public ArrayTypeInfo(TypeInfo innerType, VariableModifier innerTypeModifier)
    {
        InnerType = innerType;
        InnerTypeModifier = innerTypeModifier;
    }

    public TypeInfo InnerType { get; }
    public VariableModifier InnerTypeModifier { get; }
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
                result.Add(Variable.CreateModifier(elem.Type, InnerTypeModifier, elem));
            }

            return result;
        }

        throw new ArgumentException(ExceptionConstants.INVALID_TYPE.Format(typeof(IList<Variable>), instance.GetType()));
    }
}