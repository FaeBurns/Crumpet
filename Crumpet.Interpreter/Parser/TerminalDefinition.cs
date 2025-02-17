﻿using System.Reflection;

namespace Crumpet.Interpreter.Parser;

public sealed class TerminalDefinition<T> where T : Enum
{
    public T Token { get; }
    public ConstructorInfo Constructor { get; }

    public TerminalDefinition(T token, ConstructorInfo constructor)
    {
        Token = token;
        Constructor = constructor;
    }
}