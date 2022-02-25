using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain.Credentials
{
    public class Credentials
    {
        static string[] Scopes = { CalendarService.Scope.Calendar };
        public string ApplicationName = "Google Calendar API";
        public async Task<UserCredential> GetCredentials()
        {
            UserCredential credential;
            using (var stream =
                new FileStream("client_secret_737576027566-qppvd9jjo0jt0aqbtreqa8elc2e53k06.apps.googleusercontent.com (2).json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "admin",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            return credential;
        }
    }
}
