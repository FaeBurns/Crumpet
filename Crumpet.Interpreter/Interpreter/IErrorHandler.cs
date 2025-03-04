using Crumpet.Interpreter.Parser;

namespace Crumpet.Interpreter;

public interface IErrorHandler<in TBaseException> where TBaseException : Exception
{
    public void Throw<T>(T exception) where T : TBaseException;
}