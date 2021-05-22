using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using System.Collections.Generic;

namespace SalesUpdater.Data
{
    public interface IEmailService
    {
        UserCredential GetCredentials(string[] scopes);
        List<Message> GetEmails(string orderLabel);
        void MoveEmail(string messageDataId, string orderLabel, string newLabel);

    }
}