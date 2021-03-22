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

namespace SalesUpdater
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] Scopes = { GmailService.Scope.GmailReadonly };
            string ApplicationName = "SalesUpdater";

            UserCredential credential = GetCredentials(Scopes);

            //Create a new Gmail API Service
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            
            
            //Make a request to pull emails
            UsersResource.MessagesResource.ListRequest emailListRequest = service.Users.Messages.List("me");
            emailListRequest.LabelIds = "Label_6420272116865146";
            
            List<Message> messageDataItems = GetEmails(emailListRequest, service);

            foreach (var messageItem in messageDataItems)
                {
                    string body = messageItem.Payload.Parts[0].Body.Data;
                    String codedBody = body.Replace("-", "+");
                    codedBody = codedBody.Replace("_", "/");
                    byte[] data = Convert.FromBase64String(codedBody);
                    body = Encoding.UTF8.GetString(data);
                    Console.WriteLine(body);
                }

            
        }
        static UserCredential GetCredentials(string[] scopes)
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

        static List<Message> GetEmails(UsersResource.MessagesResource.ListRequest emailListRequest, GmailService service)
        {
        // List message.
            List<Message> messageDataItems = new List<Message>();
            IList<Message> messages = emailListRequest.Execute().Messages;
            if (messages != null && messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    var emailInfoRequest = service.Users.Messages.Get("me", message.Id);
                    Message emailData = emailInfoRequest.Execute();
                    messageDataItems.Add(emailData);
                }

                return messageDataItems;
                
            }
            else
            {
                Console.WriteLine("No messages found.");
                return messageDataItems;
            }

        }

    }
}