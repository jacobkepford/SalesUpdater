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

namespace EmailApi
{
    public class EmailService
    {
        public GmailService EmailApiService { get; set; }

        public EmailService()
        {
            string[] scopes = { GmailService.Scope.GmailReadonly };
            string applicationName = "SalesUpdater";
            
            UserCredential credential = GetCredentials(scopes);
            
            EmailApiService = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = applicationName,
        });
        
        }

        // API Request to get Email and Email Metadata
        public List<Message> GetEmails(GmailService service, UsersResource.MessagesResource.ListRequest emailListRequest)
        {
            List<Message> messageDataItems = new List<Message>();

            //Execute get email API request and save to list
            IList<Message> messages = emailListRequest.Execute().Messages;
            if (messages != null && messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    //Format a new request to get email metadata
                    var emailInfoRequest = service.Users.Messages.Get("me", message.Id);

                    //Execute request for email metadata and add to list
                    Message emailData = emailInfoRequest.Execute();
                    messageDataItems.Add(emailData);
                }
            }
            else
            {
                Console.WriteLine("No messages found.");
            }
            return messageDataItems;

        }
        
        // //Format a request to pull email id's
        // UsersResource.MessagesResource.ListRequest emailListRequest = service.Users.Messages.List("me");
        // //LabelID = Orders
        // emailListRequest.LabelIds = "Label_6420272116865146";

        // //Execute Email request to get all email metadata
        // List<Message> messageDataItems = GetEmails(service, emailListRequest);

        // //Extract email body from each email
        // List<string> messageBodies = GetMessageBodies(messageDataItems);

        // // ExtractEmailData(messageBodies);

        //Get the user credentials stored in credentials.json
        private static UserCredential GetCredentials(string[] scopes)
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