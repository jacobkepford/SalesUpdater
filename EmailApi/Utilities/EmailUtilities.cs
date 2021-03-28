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

namespace EmailApi.Utilities
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
    }
}