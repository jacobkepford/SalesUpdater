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
            //Create a new Gmail API Service
            RegisterService service = new RegisterService();
            GmailService active_service = service.NewService();

            //Make a request to pull emails
            UsersResource.MessagesResource.ListRequest emailListRequest = active_service.Users.Messages.List("me");
            emailListRequest.LabelIds = "Label_6420272116865146";


            // List message.
            IList<Message> messages = emailListRequest.Execute().Messages;
            if (messages != null && messages.Count > 0)
            {
                foreach (var messageItem in messages)
                {
                    var emailInfoRequest = active_service.Users.Messages.Get("me", messageItem.Id);
                    Message emailData = emailInfoRequest.Execute();
                }
            }
            else
            {
                Console.WriteLine("No messages found.");
            }
            Console.Read();
        }
    }
}