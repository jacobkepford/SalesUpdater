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
using EmailApi.Data;
using EmailApi.Utilities;

namespace EmailApi
{
    public class EmailService
    {
        public GmailService Connection { get; set; }
        public string Label { get; set; }

        public EmailService()
        {
            string[] scopes = { GmailService.Scope.GmailModify };
            string applicationName = "SalesUpdater";

            UserCredential credential = GetCredentials(scopes);

            Connection = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });


        }

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

        // API Request to get Email and Email Metadata
        public List<Message> GetEmails()
        {
            List<Message> messageDataItems = new List<Message>();

            //Execute get email API request and save to list
            try
            {
                //Format a request to pull email id's
                UsersResource.MessagesResource.ListRequest emailListRequest = Connection.Users.Messages.List("me");

                //Add label to request
                emailListRequest.LabelIds = Label;

                IList<Message> messages = emailListRequest.Execute().Messages;
                if (messages != null && messages.Count > 0)
                {
                    foreach (var message in messages)
                    {
                        //Format a new request to get email metadata
                        var emailInfoRequest = Connection.Users.Messages.Get("me", message.Id);

                        //Execute request for email metadata and add to list
                        Message emailData = emailInfoRequest.Execute();
                        messageDataItems.Add(emailData);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return messageDataItems;

        }

        public void MoveEmail(string messageDataId, string newLabel)
        {
            List<string> addLableIds = new List<string>();
            List<string> removeLableIds = new List<string>();
            //Label the email originated from
            removeLableIds.Add(Label);

            //Label the email should be  moved to
            addLableIds.Add(newLabel);

            try
            {
                //Write the body of the request to add/remove label
                ModifyMessageRequest mods = new ModifyMessageRequest();
                mods.RemoveLabelIds = removeLableIds;
                mods.AddLabelIds = addLableIds;

                //Format/execute the full request to move the email properly
                UsersResource.MessagesResource.ModifyRequest moveEmailRequest = Connection.Users.Messages.Modify(mods, "me", messageDataId);
                moveEmailRequest.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

    }
}