namespace Crumpet.Interpreter.Parser.NodeConstraints;

public abstract class NodeConstraint
{
    public bool IncludeInConstructor { get; set; }

    protected NodeConstraint(bool includeInConstructor)
    {
        IncludeInConstructor = includeInConstructor;
    }

    public abstract override string ToString();
}