﻿namespace Crumpet.Interpreter.Parser.Elements;

public abstract class ParserElement
{
    public abstract IEnumerable<object> TransformForConstructor();
}