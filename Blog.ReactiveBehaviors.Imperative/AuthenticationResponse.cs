using System;

namespace Blog.ReactiveBehaviors.Imperative
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse(string authenticationToken)
        {
            AuthenticationToken = authenticationToken;
            Successful = true;
        }

        public AuthenticationResponse(Exception error)
        {
            Error = error;
            Successful = false;
        }

        public bool Successful { get; private set; }
        public string AuthenticationToken { get; private set; }
        public Exception Error { get; private set; }
    }
}