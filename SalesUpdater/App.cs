using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using SalesUpdater.Data;
using SalesUpdater.Core;
using SalesUpdater.Data.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SalesUpdater
{
    public class App : IApp
    {
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly ISheetService _sheetService;

        public App(ILogger<App> logger, IEmailService emailService, ISheetService sheetService)
        {
            _logger = logger;
            _emailService = emailService;
            _sheetService = sheetService;
        }

        public void Run()
        {
            //Set order and processed labels
            string orderLabel = "Label_6420272116865146";
            string processedLabel = "Label_5885438401785530646";

            //Execute Email request to get all email metadata
            List<Message> messageDataItems = _emailService.GetEmails(orderLabel);

            if (messageDataItems == null || messageDataItems.Count == 0)
            {
                _logger.LogInformation("No emails were found");
                Console.WriteLine("No emails were found");
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
                string result = _sheetService.CreateEntry(email, sheet);

                //Check to verify that sheet entry was added properly
                if (result == "Success")
                {
                    _emailService.MoveEmail(messageDataItems[emailCount].Id, orderLabel, processedLabel);
                }
                else
                {
                    failCount++;
                }

                emailCount++;
            }

            _logger.LogInformation($"All emails have been processed with {failCount} error(s).");
            Console.WriteLine($"All emails have been processed with {failCount} error(s).");
        }
    }
}