using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SalesUpdater.Data
{
    public interface IEmailService
    {
        UserCredential GetCredentials(string[] scopes);
        List<Message> GetEmails(string orderLabel);
        void MoveEmail(string messageDataId, string orderLabel, string newLabel);

    }
}