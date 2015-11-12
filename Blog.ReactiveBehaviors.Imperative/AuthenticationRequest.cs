namespace Blog.ReactiveBehaviors.Imperative
{
    public class AuthenticationRequest
    {
        public AuthenticationRequest(string _username, string _password)
        {
            Username = _username;
            Password = _password;
        }

        public string Password { get; private set; }
        public string Username { get; private set; }
    }
}