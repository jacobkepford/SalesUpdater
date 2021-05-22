using Google.Apis.Auth.OAuth2;
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