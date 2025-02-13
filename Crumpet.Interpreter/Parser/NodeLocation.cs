namespace Crumpet.Interpreter.Parser;

public class NodeLocation
{
    public NodeLocation()
    {
    }
    
    public NodeLocation(int startOffset, int endOffset, int startLine, int endLine, int startColumn, int endColumn)
    {
        StartOffset = startOffset;
        EndOffset = endOffset;
        StartLine = startLine;
        EndLine = endLine;
        StartColumn = startColumn;
        EndColumn = endColumn;
    }

    public int StartOffset;
    public int EndOffset;
    public int StartLine;
    public int EndLine;
    public int StartColumn;
    public int EndColumn;
}