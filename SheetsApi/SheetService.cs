using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SheetsApi
{
    public class SheetService
    {
        public SheetsService Connection { get; set; }
        public SheetService()
        {
            string[] scopes = { SheetsService.Scope.Spreadsheets };
            string applicationName = "SalesUpdater";

            UserCredential credential = GetCredentials(scopes);

            Connection = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        private UserCredential GetCredentials(string[] scopes)
        {
            UserCredential credential;
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            return credential;

        }
    }
}