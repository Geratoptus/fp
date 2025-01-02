using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace FileSenderRailway;

public class FileSender(
    ISender sender,
    Func<DateTime> now,
    IRecognizer recognizer,
    ICryptographer cryptographer)
{
    public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
        => files.Select(file => new FileSendResult(file, recognizer
            .Recognize(file).AsResult()
            .Then(doc => PrepareDocumentToSend(doc, certificate))
            .RefineError("Can't prepare file to send")
            .Then(sender.Send).Error
        ));

    private Result<Document> CheckDocumentVersion(Document document)
        => IsValidFormatVersion(document) ? document : Result.Fail<Document>("Invalid format version");
    
    private Result<Document> CheckDocumentTimestamp(Document document) 
        => IsValidTimestamp(document) ? document : Result.Fail<Document>("Too old document");

    private Result<Document> PrepareDocumentToSend(Document doc, X509Certificate certificate)
        => doc.AsResult()
            .Then(CheckDocumentVersion)
            .Then(CheckDocumentTimestamp)
            .Then(d => d.ChangeContent(cryptographer.Sign(d.Content, certificate)));
    

    private bool IsValidFormatVersion(Document doc) 
        => doc.Format is "4.0" or "3.1";

    private bool IsValidTimestamp(Document doc)
        => doc.Created > now().AddMonths(-1);
}