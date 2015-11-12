using System;
using Caliburn.Micro;

namespace Blog.ReactiveBehaviors.Imperative
{
    public interface ILogInPageViewModel : IScreen
    {
    }

    public class LogInPageViewModel : Screen, ILogInPageViewModel
    {
        private string _username;
        private string _password;

        private void CheckIfLogInButtonShouldBeEnabled()
        {
            if (!string.IsNullOrWhiteSpace(_username) && !string.IsNullOrWhiteSpace(_password))
            {

            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (!string.Equals(value, _username))
                {
                    _username = value;

                    NotifyOfPropertyChange(() => Username);

                    CheckIfLogInButtonShouldBeEnabled();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (!string.Equals(value, _password))
                {
                    _password = value;

                    NotifyOfPropertyChange(() => Password);

                    CheckIfLogInButtonShouldBeEnabled();
                }
            }
        }
    }
}