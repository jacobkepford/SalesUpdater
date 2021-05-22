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

namespace SalesUpdater.Data
{
    public interface ISheetService
    {
        UserCredential GetCredentials(string[] scopes);
        void ReadEntries(Worksheet sheet);
        string CreateEntry(Email email, Worksheet sheet);
        string GetNextID(Worksheet sheet);

    }
}