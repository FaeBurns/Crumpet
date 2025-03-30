using System.Diagnostics;
using System.Text;

namespace Shared;

public class TextSliceReader
{
    private readonly StreamReader m_streamReader;
    
    public TextSliceReader(Stream stream)
    {
        m_streamReader = new StreamReader(stream);
    }

    public ReadOnlySpan<char> Read(int startOffset, int endOffset)
    {
        m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
        char[] buffer = new char[4096];
        int desiredLength = endOffset - startOffset;
        char[] outputBuffer = new char[desiredLength];
        int position = 0;
        while (true)
        {
            // read into buffer n characters
            int read = m_streamReader.Read(buffer, 0, 4096);
            
            // skip to next loop if we're not yet there
            if (position + read < startOffset)
            {
                position += read;
                continue;
            }
            
            int beginCopyOffset = startOffset - position;
            int amountToCopy = desiredLength - beginCopyOffset;
            Array.Copy(buffer, beginCopyOffset, outputBuffer, 0, amountToCopy);
            
            // populate the rest of the output buffer
            int amountLeftToRead = desiredLength - amountToCopy;
            ReadExactly(outputBuffer, amountToCopy, amountLeftToRead);

            return new ReadOnlySpan<char>(outputBuffer);
        }
    }

    private void ReadExactly(char[] buffer, int offset, int amount)
    {
        if (amount == 0)
            return;
        
        int read = 0;
        while (read < amount)
        {
            read += m_streamReader.Read(buffer, offset + read, amount - read);
        }
    }

    public void Dispose()
    {
        m_streamReader.Dispose();
    }
}