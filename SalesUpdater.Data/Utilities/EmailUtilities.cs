using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SalesUpdater.Core;

namespace SalesUpdater.Data.Utilities
{
    public static class EmailUtilities
    {

        public static string EmailSearch(string text, string expr)
        {
            Match m = Regex.Match(text, expr);
            Group g = m.Groups[1];
            return g.ToString();
        }

        //Pull email body from each email supplied
        public static List<string> EmailBodyCleanup(List<Message> messageDataItems)
        {
            List<string> messageBodies = new List<string>();

            foreach (var messageItem in messageDataItems)
            {
                string body = messageItem.Payload.Parts[0].Body.Data;
                string codedBody = body.Replace("-", "+");
                codedBody = codedBody.Replace("_", "/");
                byte[] data = Convert.FromBase64String(codedBody);
                body = Encoding.UTF8.GetString(data);
                body = body.Replace("\r\n", "");
                messageBodies.Add(body);
            }
            return messageBodies;
        }
        //Pulls key order data from supplied emails
        public static List<Email> ExtractEmailData(List<string> messageBodies)
        {
            List<Email> emails = new List<Email>();

            foreach (var message in messageBodies)
            {
                Email email = new Email();

                //Format and run regex search for Order ID
                string orderIDExpr = "Order: \\#([0-9]+)";
                email.OrderNumber = EmailSearch(message, orderIDExpr);

                //Format and run regex search for product
                string orderProductExpr = "Price(.*) [\\d]* \\$";
                email.Product = EmailSearch(message, orderProductExpr);

                //Format and run regex search for Quantity
                string orderQuantity = "Price.* ([\\d]*) \\$";
                email.Quantity = EmailSearch(message, orderQuantity);

                //Format and run regex search for person who placed order
                string orderPersonNameExpr = "order from ([a-zA-z]* [a-zA-Z?][a-zA-z]*):";
                email.OrderPerson = EmailSearch(message, orderPersonNameExpr);

                //Format and run regex search for date order was placed
                string orderDateExpr = "\\(([A-Z][a-z]+[0-9]*, [0-9]{4})\\)Product";
                string emailOrderDate = EmailSearch(message, orderDateExpr);
                string pattern = "([A-Z][a-z]+)([0-9]*,)";
                string replacement = "$1" + " " + "$2";
                email.OrderDate = DateTime.Parse(Regex.Replace(emailOrderDate, pattern, replacement));

                //Format and run regex search for email address
                string emailAddressExpr = "[0-9]{10}>(.*@.*\\.com)";
                email.EmailAddress = EmailSearch(message, emailAddressExpr);

                //Format and run regex search for Payment Method
                string paymentMethodExpr = "method: (.*)Total";
                string payment = EmailSearch(message, paymentMethodExpr);

                if (payment == "Credit Card")
                {
                    email.PaymentMethod = "Stripe";
                }
                else if (payment == "PayPal")
                {
                    email.PaymentMethod = "PayPal";
                }
                else
                {
                    email.PaymentMethod = "Not Found";
                }

                //Format and run regex search for Subtotal
                string subtotalExpr = "Subtotal: (\\$[0-9]*,?[0-9]*?\\.[0-9]{2})Discount";
                email.Subtotal = EmailSearch(message, subtotalExpr);

                //Format and run regex for total
                string totalExpr = "Total: (\\$[0-9]*,?[0-9]*?\\.[0-9]{2})Order";
                email.Total = EmailSearch(message, totalExpr);

                emails.Add(email);

            }



            return emails;
        }
    }
}