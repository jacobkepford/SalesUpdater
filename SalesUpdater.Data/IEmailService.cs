using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using SalesUpdater.Core;
using System.Collections.Generic;

namespace SalesUpdater.Data
{
    public interface IEmailService
    {
        UserCredential GetCredentials(string[] scopes);
        List<Message> GetRawEmails(string orderLabel);
        List<Email> GetEmailBodies(List<Message> messageDataItems);
        void MoveEmail(string messageDataId, string orderLabel, string newLabel);

    }
}