using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Exceptions;

public class TypeMismatchException : Exception
{
    public TypeMismatchException(Type expected, Type encountered) : base(ExceptionConstants.INVALID_TYPE.Format(expected, encountered))
    {
    }

    public TypeMismatchException(TypeInfo expected, TypeInfo encountered) : base(ExceptionConstants.INVALID_TYPE.Format(expected, encountered))
    {
    }
}