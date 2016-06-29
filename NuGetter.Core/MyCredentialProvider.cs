namespace NuGetter.Core {
    using System;
    using System.Net;
    using NuGet;

    public class MyCredentialProvider : ICredentialProvider {
        private readonly string _username;
        private readonly string _password;

        public MyCredentialProvider(string username, string password) {
            _username = username;
            _password = password;
        }

        public ICredentials GetCredentials(Uri uri, IWebProxy proxy, CredentialType credentialType, bool retrying) {
            return new NetworkCredential(_username, _password);
        }
    }
}