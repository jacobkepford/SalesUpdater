using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using Google.Apis.Gmail.v1.Data;
using System.Text;
using SalesUpdater.Core;
using Microsoft.Extensions.Logging;

namespace SalesUpdater.Data
{
    public class EmailUtilities : IEmailUtilities
    {
        private readonly ILogger<EmailUtilities> _logger;

        public EmailUtilities(ILogger<EmailUtilities> logger)
        {
            _logger = logger;
        }
        public string EmailSearch(string text, string expr)
        {
            Match m = Regex.Match(text, expr);
            Group g = m.Groups[1];
            return g.ToString();
        }

        public string StripHtml(string body)
        {
            return Regex.Replace(body, "<.*?>", String.Empty);
        }

        //Pull email body from each email supplied
        public List<string> EmailBodyCleanup(List<Message> messageDataItems)
        {
            List<string> messageBodies = new List<string>();
            string body = "";

            foreach (var messageItem in messageDataItems)
            {
                if (messageItem.Payload.Parts == null)
                {
                    body = messageItem.Payload.Body.Data;
                }
                else
                {
                    body = messageItem.Payload.Parts[0].Body.Data;
                }
                string codedBody = body.Replace("-", "+");
                codedBody = codedBody.Replace("_", "/");
                byte[] data = Convert.FromBase64String(codedBody);
                body = Encoding.UTF8.GetString(data);
                body = StripHtml(body);
                body = body.Replace("\t", "");
                body = body.Replace("\n", "");
                body = body.Replace("&amp;", "&");
                messageBodies.Add(body);

            }
            return messageBodies;
        }
        //Pulls key order data from supplied emails
        public List<Email> ExtractEmailData(List<string> messageBodies)
        {
            List<Email> emails = new List<Email>();

            foreach (var message in messageBodies)
            {
                Email email = new Email();

                //Format and run regex search for Order ID
                string orderIDExpr = "Order: \\#([0-9]+)";
                email.OrderNumber = EmailSearch(message, orderIDExpr);

                //Format and run regex search for product
                string orderProductExpr = "QuantityPrice(.*)[\\d]{1,3}\\$";
                email.Product = EmailSearch(message, orderProductExpr);

                //Format and run regex search for Quantity
                string orderQuantity = "Price.*([\\d]{1,3})\\$";
                email.Quantity = EmailSearch(message, orderQuantity);

                //Format and run regex search for person who placed order
                string orderPersonNameExpr = "order from ([a-zA-z]* [a-zA-Z]. ?[a-zA-z]*):";
                email.OrderPerson = EmailSearch(message, orderPersonNameExpr);

                //Format and run regex search for date order was placed
                string orderDateExpr = "\\(([A-Z][a-z]+ [0-9]*, [0-9]{4})\\)Product";
                string emailOrderDate = EmailSearch(message, orderDateExpr);
                if (emailOrderDate == "")
                {
                    emailOrderDate = "January 1, 2001";
                }
                string pattern = "([A-Z][a-z]+)([0-9]*,)";
                string replacement = "$1" + " " + "$2";
                email.OrderDate = DateTime.Parse(Regex.Replace(emailOrderDate, pattern, replacement));

                //Format and run regex search for email address
                string emailAddressExpr = "address.*[0-9]+([a-zA-Z].*@.*\\.[a-zA-Z]+)Shipping";
                email.EmailAddress = EmailSearch(message, emailAddressExpr);

                //Format and run regex search for Payment Method
                string paymentMethodExpr = "method:(.*)Total";
                string payment = EmailSearch(message, paymentMethodExpr);

                if (payment == "Credit Card")
                {
                    email.PaymentMethod = "Stripe";
                }
                else
                {
                    email.PaymentMethod = payment;
                }

                //Format and run regex search for Subtotal
                string subtotalExpr = "Subtotal:(.*\\.\\d{2})Shipping";
                email.Subtotal = EmailSearch(message, subtotalExpr);

                //Format and run regex for total
                string totalExpr = "Total:(.*\\.\\d{2})Order";
                email.Total = EmailSearch(message, totalExpr);

                emails.Add(email);

            }

            return emails;
        }

        public string GetOrderID(string message)
        {
            string orderIDExpr = "Order: \\#([0-9]+)";
            string orderID = EmailSearch(message, orderIDExpr);
            return orderID;
        }
    }
}