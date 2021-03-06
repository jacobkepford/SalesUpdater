using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SalesUpdater.Core;
using Microsoft.Extensions.Logging;

namespace SalesUpdater.Data
{
    public class SheetService : ISheetService
    {
        private readonly ILogger<SheetService> _log;

        public SheetsService Connection { get; set; }

        //Intializes new service with credentials and API connection
        public SheetService(ILogger<SheetService> log)
        {
            _log = log;
            string[] scopes = { SheetsService.Scope.Spreadsheets };
            string applicationName = "SalesUpdater";

            UserCredential credential = GetCredentials(scopes);

            Connection = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

        }

        //Gets the credentials to use for the service
        public UserCredential GetCredentials(string[] scopes)
        {
            UserCredential credential;
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "SheetToken.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                _log.LogInformation("Sheet credential file saved to: " + credPath);
            }
            return credential;

        }

        //Method set up for testing reading values from Google Sheet
        public void ReadEntries(Worksheet sheet)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    Connection.Spreadsheets.Values.Get(sheet.WorksheetID, sheet.WorkingRange);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    // Print columns A to F, which correspond to indices 0 and 4.
                    Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5}", row[0], row[1], row[2], row[3], row[4], row[5]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }

        //Writes a new row into the Google Sheet
        public string CreateEntry(Email email, Worksheet sheet)
        {
            string result = "";

            try
            {
                var valueRange = new ValueRange();

                string rowID = GetNextID(sheet);

                var oblist = new List<object>() { rowID, email.OrderPerson, email.OrderDate.ToString("MM-dd-yyyy"), email.EmailAddress, email.PaymentMethod, email.OrderNumber, email.Product, email.Quantity, email.Subtotal, email.Total };

                valueRange.Values = new List<IList<object>> { oblist };

                var appendRequest = Connection.Spreadsheets.Values.Append(valueRange, sheet.WorksheetID, sheet.WorkingRange);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                var appendReponse = appendRequest.Execute();

                result = "Success";
            }
            catch (Exception e)
            {
                _log.LogInformation($"Unable to process Order #: {email.OrderNumber}");
                _log.LogInformation(e.Message);
                result = "Fail";
            }

            return result;

        }

        //Finds the next SheetID to use for inserting a new record
        public string GetNextID(Worksheet sheet)
        {
            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        Connection.Spreadsheets.Values.Get(sheet.WorksheetID, sheet.WorkingRange);

                var response = request.Execute();
                IList<IList<object>> values = response.Values;
                if (values != null && values.Count > 1)
                {
                    //Returns the last ID entered into the spreadsheet. Adding 1 to increment
                    int currentLastID = Convert.ToInt32(values[values.Count - 1][0]);
                    string nextID = (currentLastID + 1).ToString();
                    return nextID;
                }

            }
            catch (Exception e)
            {
                _log.LogInformation("Unable to get next ID");
                _log.LogInformation(e.Message);
            }
            return "1";

        }
    }
}