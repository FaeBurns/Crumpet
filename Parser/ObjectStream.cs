using System.Collections;
using System.Diagnostics;

namespace Parser;

public class ObjectStream<T> : IEnumerator<T>, IEnumerable<T>
{
    private readonly IReadOnlyList<T> m_objectList;
    private int m_position = 0;

    T IEnumerator<T>.Current => m_objectList[Position];

    object? IEnumerator.Current => m_objectList[Position];
    
    public int Length => m_objectList.Count;
    
    public int HighestPosition { get; private set; }

    public int Position
    {
        get => m_position;
        set
        {
            m_position = value;
            if (m_position > HighestPosition)
                HighestPosition = m_position;
        }
    }

    public bool IsAtEnd => Position == Length;

    public T this[int index] => m_objectList[index];

    public ObjectStream(IEnumerable<T> objects)
    {
        m_objectList = new List<T>(objects);
    }
    
    public T ReadNext()
    {
        T value = PeekCurrent();
        Position++;
        return value;
    }

    public T PeekCurrent()
    {
        if (Position >= m_objectList.Count)
            throw new IndexOutOfRangeException("Index out of bounds of stream");
        return m_objectList[Position];
    }

    public int Seek(int amount, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                Position = amount;
                break;
            case SeekOrigin.Current:
                Position += amount;
                break;
            case SeekOrigin.End:
                Position = m_objectList.Count + amount;
                break;
            default:
                throw new UnreachableException();
        }

        return Position;
    }

    public bool MoveNext()
    {
        Position++;

        // if position is now outside the bounds of the collection then return false
        if (Position >= m_objectList.Count)
            return false;

        return true;
    }

    public void Reset()
    {
        Position = 0;
    }

    public void Dispose()
    {
    }

    public IEnumerator<T> GetEnumerator()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this;
    }

    public PositionSaver<T> ConstrainPosition()
    {
        return new PositionSaver<T>(this);
    }
    
    public override string ToString()
    {
        return $"({Position}:{m_objectList[Position]?.ToString()})";
    }
}

public class PositionSaver<T> : IDisposable
{
    private readonly ObjectStream<T> m_stream;
    
    public int Position { get; }
    public bool ResetsPosition { get; set; } = true;

    public PositionSaver(ObjectStream<T> stream)
    {
        m_stream = stream;
        Position = stream.Position;
    }
    
    public void ConsumePosition() => ResetsPosition = false;
    
    public void Dispose()
    {
        if (ResetsPosition)
            m_stream.Position = Position;
    }
}