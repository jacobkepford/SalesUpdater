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
using EmailApi;

namespace SalesUpdater
{
    class Program
    {

        static void Main(string[] args)
        {
            
            
            EmailService emailService = new EmailService();



            //Format a request to pull email id's
            UsersResource.MessagesResource.ListRequest emailListRequest = emailService.EmailApiService.Users.Messages.List("me");
            //LabelID = Orders
            emailListRequest.LabelIds = "Label_6420272116865146";

            //Execute Email request to get all email metadata
            List<Message> messageDataItems = emailService.GetEmails(emailService.EmailApiService, emailListRequest);

            // //Extract email body from each email
            // List<string> messageBodies = GetMessageBodies(messageDataItems);

            // // ExtractEmailData(messageBodies);


        }

        

        //API Request to get Email and Email Metadata
        // private static List<Message> GetEmails(GmailService service, UsersResource.MessagesResource.ListRequest emailListRequest)
        // {
        //     List<Message> messageDataItems = new List<Message>();

        //     //Execute get email API request and save to list
        //     IList<Message> messages = emailListRequest.Execute().Messages;
        //     if (messages != null && messages.Count > 0)
        //     {
        //         foreach (var message in messages)
        //         {
        //             //Format a new request to get email metadata
        //             var emailInfoRequest = service.Users.Messages.Get("me", message.Id);

        //             //Execute request for email metadata and add to list
        //             Message emailData = emailInfoRequest.Execute();
        //             messageDataItems.Add(emailData);
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("No messages found.");
        //     }
        //     return messageDataItems;

        // }

        // //Pull email body from each email supplied
        // private static List<string> GetMessageBodies(List<Message> messageDataItems)
        // {
        //     List<string> messageBodies = new List<string>();

        //     foreach (var messageItem in messageDataItems)
        //     {
        //         string body = messageItem.Payload.Parts[0].Body.Data;
        //         string codedBody = body.Replace("-", "+");
        //         codedBody = codedBody.Replace("_", "/");
        //         byte[] data = Convert.FromBase64String(codedBody);
        //         body = Encoding.UTF8.GetString(data);
        //         body = body.Replace("\r\n", "");
        //         messageBodies.Add(body);
        //     }
        //     return messageBodies;
        // }

        // // private static void ExtractEmailData(List<string> messageBodies)
        // // {
        // //     foreach (var message in messageBodies)
        // //     {
        // //         Email email = new Email();

        // //         //Format and run regex search for Order ID
        // //         string orderIDExpr = "Order: \\#([0-9]+)";
        // //         email.OrderNumber = ShowMatch(message, orderIDExpr);

        // //         //Format and run regex search for product
        // //         string orderProductExpr = "Price(.*) [0-9] \\$";
        // //         email.Product = ShowMatch(message, orderProductExpr);
        // //         Console.WriteLine(email.Product);

        // //     }
        // // }

        // private static string ShowMatch(string text, string expr)
        // {
        //     Match m = Regex.Match(text, expr);
        //     Group g = m.Groups[1];
        //     return g.ToString();

        // }
    }
}