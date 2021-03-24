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
using System.Text.RegularExpressions;
using EmailApi;
using EmailApi.Data;

namespace SalesUpdater
{
    class Program
    {

        static void Main(string[] args)
        {
            
            
            EmailService emailService = new EmailService();
            
            //Set Label as "Orders"
            emailService.Label = "Label_6420272116865146";

            //Format a request to pull email id's
            UsersResource.MessagesResource.ListRequest emailListRequest = emailService.Connection.Users.Messages.List("me");
            //Add label to request
            emailListRequest.LabelIds = emailService.Label;

            //Execute Email request to get all email metadata
            List<Message> messageDataItems = emailService.GetEmails(emailListRequest);

            //Extract email body from each email
            List<string> messageBodies = emailService.GetMessageBodies(messageDataItems);

            List<Email> emails = emailService.ExtractEmailData(messageBodies);


        }

    }
}