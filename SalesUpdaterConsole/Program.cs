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
using EmailApi;
using EmailApi.Data;
using EmailApi.Utilities;
using SheetsApi;


namespace SalesUpdater
{
    class Program
    {

        static void Main(string[] args)
        {

            EmailService emailService = new EmailService();
            SheetService sheetService = new SheetService();

            //Set Label as "Orders"
            emailService.Label = "Label_6420272116865146";

            //Execute Email request to get all email metadata
            List<Message> messageDataItems = emailService.GetEmails();

            //Extract email body from each email
            List<string> messageBodies = EmailUtilities.EmailBodyCleanup(messageDataItems);

            //Extract key email data from each email body
            List<Email> emails = EmailUtilities.ExtractEmailData(messageBodies);

            //Initializing Google Sheet Information
            sheetService.SpreadsheetID = "1v9GJRu5CwjXW_r2ELlHbjujTUdDj27DMxLb4lutI5Ug";
            sheetService.Sheet = "Sales";
            sheetService.Range = "A:J";

            //Writes email data to google sheet
            foreach (Email email in emails)
            {
                sheetService.CreateEntry(email);
            }


            //Request to move email to proceesed. Adding "processed" label id
            string newLabel = "Label_5885438401785530646";
            foreach (Message message in messageDataItems)
            {
                emailService.MoveEmail(message.Id, newLabel);
            }



        }

    }
}