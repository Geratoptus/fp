using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;

namespace FileSenderRailway;

public interface ICryptographer
{
    byte[] Sign(byte[] content, X509Certificate certificate);
}

public interface IRecognizer
{
    /// <exception cref="FormatException">Not recognized</exception>
    Document Recognize(FileContent file);
}

public interface ISender
{
    /// <exception cref="InvalidOperationException">Can't send</exception>
    void Send(Document document);
}

public record Document(string Name, byte[] Content, DateTime Created, string Format)
{
    public Document ChangeContent(byte[] newContent)
        => this with { Content = newContent };
}

public class FileContent
{
    public FileContent(string name, byte[] content)
    {
        Name = name;
        Content = content;
    }

    public string Name { get; }
    public byte[] Content { get; }
}

public class FileSendResult
{
    public FileSendResult(FileContent file, string error = null)
    {
        File = file;
        Error = error;
    }

    public FileContent File { get; }
    public string Error { get; }
    public bool IsSuccess => Error == null;
}