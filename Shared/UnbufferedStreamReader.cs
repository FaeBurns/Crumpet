using System.Text;

namespace Shared;

public class UnbufferedStreamReader : TextReader
{
    private readonly Stream m_stream;

    public UnbufferedStreamReader(Stream stream)
    {
        m_stream = stream;
    }

    public override string? ReadLine()
    {
        List<byte> bytes = new List<byte>();
        int current;

        while ((current = m_stream.ReadByte()) != -1 && current != '\n')
        {
            bytes.Add((byte)current);
        }

        if (bytes.Count == 0)
            return null;
        
        return Encoding.UTF8.GetString(bytes.ToArray());
    }
}