namespace Crumpet.Interpreter.Parser;

public class SourceLocation
{
    public static SourceLocation FromRange(SourceLocation start, SourceLocation end)
    {
        return new SourceLocation(start.StartOffset, end.EndOffset, start.StartLine, end.EndLine, start.StartColumn, end.EndColumn);
    }
    
    public SourceLocation()
    {
    }
    
    public SourceLocation(int startOffset, int endOffset, int startLine, int endLine, int startColumn, int endColumn)
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

    public override string ToString()
    {
        return $"Location {StartLine + 1}:{StartColumn + 1} - {EndLine + 1}:{EndColumn + 1} ({StartOffset} - {EndOffset})";
    }
}