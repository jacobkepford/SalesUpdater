using System.Collections.Generic;
using Google.Apis.Gmail.v1.Data;
using SalesUpdater.Core;

namespace SalesUpdater.Data
{
    public interface IEmailUtilities
    {
        string EmailSearch(string text, string expr);
        string StripHtml(string body);
        List<string> EmailBodyCleanup(List<Message> messageDataItems);
        List<Email> ExtractEmailData(List<string> messageBodies);
        string GetOrderID(string message);

    }
}