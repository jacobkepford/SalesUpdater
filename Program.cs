﻿using Google.Apis.Auth.OAuth2;
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
            UserCredential credential;
            
            RegisterService service = new RegisterService();
            credential = service.GetCredentials();
            GmailService running_service = service.StartService(credential);

            UsersResource.LabelsResource.ListRequest request = running_service.Users.Labels.List("rtbredge@gmail.com");

            // List labels.
            IList<Label> labels = request.Execute().Labels;
            Console.WriteLine("Labels:");
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }
            Console.Read();
        }
    }
}