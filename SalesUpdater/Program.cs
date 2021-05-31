using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using SalesUpdater.Data;
using SalesUpdater.Core;
using SalesUpdater.Data.Utilities;
using Microsoft.Extensions.DependencyInjection;


namespace SalesUpdater
{
    class Program
    {

        static void Main(string[] args)
        {
            //Create instance of Email / Sheet Service through Dependency Injection
            var host = Startup.AppStartup();

            var emailService = ActivatorUtilities.CreateInstance<EmailService>(host.Services);
            var sheetService = ActivatorUtilities.CreateInstance<SheetService>(host.Services);

            //Set order and processed labels
            string orderLabel = "Label_6420272116865146";
            string processedLabel = "Label_5885438401785530646";

            //Execute Email request to get all email metadata
            List<Message> messageDataItems = emailService.GetEmails(orderLabel);

            if (messageDataItems == null || messageDataItems.Count == 0)
            {
                return;
            }

            //Extract email body from each email
            List<string> messageBodies = EmailUtilities.EmailBodyCleanup(messageDataItems);

            //Extract key email data from each email body
            List<Email> emails = EmailUtilities.ExtractEmailData(messageBodies);

            //Initializing Google Sheet Information
            Worksheet sheet = new Worksheet();
            sheet.WorksheetID = "1v9GJRu5CwjXW_r2ELlHbjujTUdDj27DMxLb4lutI5Ug";
            sheet.Name = "Sales";
            sheet.Range = "A:J";
            sheet.WorkingRange = $"{sheet.Name}!{sheet.Range}";

            //Count to keep track of current email id
            int emailCount = 0;

            //Count to keep track of failures
            int failCount = 0;


            foreach (Email email in emails)
            {
                //Writes email data to google sheet
                string result = sheetService.CreateEntry(email, sheet);

                //Check to verify that sheet entry was added properly
                if (result == "Success")
                {
                    emailService.MoveEmail(messageDataItems[emailCount].Id, orderLabel, processedLabel);
                }
                else
                {
                    failCount++;
                }

                emailCount++;
            }

            Console.WriteLine($"All emails have been processed with {failCount} error(s).");

        }

    }
}