using Caliburn.Micro;
using System;
using System.Threading;
using System.Windows.Input;

namespace Blog.ReactiveBehaviors.Imperative
{
    public interface ILogInPageViewModel : IScreen, IDisposable
    {
        string Username {get; set; }
        string Password { get; set; }
        string Error { get; }
        ICommand LogInCommand { get; }
        ICommand CancelCommand { get; }
    }

    public class LogInPageViewModel : Screen, ILogInPageViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly DelegateCommand _logInCommand;
        private readonly DelegateCommand _cancelCommand;

        private CancellationTokenSource _cts;

        private string _username;
        private string _password;
        private string _error;

        public LogInPageViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            _cts = new CancellationTokenSource();
            _logInCommand = new DelegateCommand(CanLogIn, PerformLogIn);
            _cancelCommand = new DelegateCommand(_ => true, CancelLogIn);
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        private bool CanLogIn(object parameter)
        {
            return (!string.IsNullOrWhiteSpace(_username) && !string.IsNullOrWhiteSpace(_password));
        }

        private async void PerformLogIn(object parameter)
        {
            AuthenticationResponse response = await _authenticationService.AuthenticateAsync(new AuthenticationRequest(_username, _password), _cts.Token);

            if (response.Successful)
            {
                TryClose(true);
            }
            else
            {
                Error = response.Error.Message;
            }
        }

        private void CancelLogIn(object parameter)
        {
            TryClose(false);
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

                    _logInCommand.RaiseCanExecuteChanged();
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
                    
                    _logInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Error
        {
            get { return _error; }
            private set
            {
                if (!string.Equals(value, _error))
                {
                    _error = value;

                    NotifyOfPropertyChange(() => Error);
                }
            }
        }

        public ICommand LogInCommand
        {
            get { return _logInCommand; }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }
    }
}