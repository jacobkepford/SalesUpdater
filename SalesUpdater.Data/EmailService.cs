using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Logging;
using SalesUpdater.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace SalesUpdater.Data
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _log;
        private readonly IEmailUtilities _emailUtilities;

        public GmailService Connection { get; set; }



        public EmailService(ILogger<EmailService> log, IEmailUtilities emailUtilities)
        {
            _log = log;
            _emailUtilities = emailUtilities;
            string[] scopes = { GmailService.Scope.GmailModify };
            string applicationName = "SalesUpdater";

            UserCredential credential = GetCredentials(scopes);

            Connection = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        public UserCredential GetCredentials(string[] scopes)
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
                _log.LogInformation("Email credential file saved to: " + credPath);
            }
            return credential;

        }

        // API Request to get Email and Email Metadata
        public List<Message> GetRawEmails(string processedLabel)
        {
            List<Message> messageDataItems = new List<Message>();

            //Execute get email API request and save to list
            try
            {
                //Format a request to pull email id's
                UsersResource.MessagesResource.ListRequest emailListRequest = Connection.Users.Messages.List("me");

                //Add label to request
                emailListRequest.LabelIds = processedLabel;

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
                    return messageDataItems;
                }


            }
            catch (Exception e)
            {
                _log.LogInformation("Error when trying to get emails");
                _log.LogInformation(e.Message);
            }

            return messageDataItems;

        }

        public void MoveEmail(string messageDataId, string orderLabel, string processedLabel)
        {
            List<string> addLableIds = new List<string>();
            List<string> removeLableIds = new List<string>();
            //Label the email originated from
            removeLableIds.Add(orderLabel);

            //Label the email should be  moved to
            addLableIds.Add(processedLabel);

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
                _log.LogInformation("Error when trying to move email to processed folder");
                _log.LogInformation(e.Message);
            }


        }

        public List<Email> GetEmailBodies(List<Message> messageDataItems)
        {
            //Extract email body from each email
            List<string> messageBodies = _emailUtilities.EmailBodyCleanup(messageDataItems);

            //Extract key email data from each email body
            List<Email> emails = _emailUtilities.ExtractEmailData(messageBodies);

            return emails;
        }
    }
}