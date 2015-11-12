using Caliburn.Micro;
using Caliburn.Micro.Reactive.Extensions;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace Blog.ReactiveBehaviors.Declarative
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

        private ObservableProperty<string> _username;
        private ObservableProperty<string> _password;
        private ObservableProperty<string> _error;
        private ObservableCommand _logInCommand;
        private ObservableCommand _cancelCommand;

        private Subject<AuthenticationResponse> _logInResponse;

        private IDisposable _behaviors;

        public LogInPageViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            _username = new ObservableProperty<string>(this, () => Username);
            _password = new ObservableProperty<string>(this, () => Password);
            _error = new ObservableProperty<string>(this, () => Error);
            _logInCommand = new ObservableCommand();
            _cancelCommand = new ObservableCommand();

            _logInResponse = new Subject<AuthenticationResponse>();

            _behaviors = new CompositeDisposable(
                WhenTheUserHasEnteredBothUsernameAndPasswordThenEnableLogInButton(),
                WhenTheUserClicksTheLogInButtonAttemptToLogIn(),
                WhenASuccessfulLogInAttemptIsMadeCloseTheDialog(),
                WhenAnUnsuccessfulLogInAttemptIsMadeDisplayTheError(),
                WhenTheUserClicksTheCancelButtonCloseTheDialog()
            );
        }

        private IDisposable WhenTheUserHasEnteredBothUsernameAndPasswordThenEnableLogInButton()
        {
            return Observable
                .CombineLatest(_username, _password, (username, password) => !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                .Subscribe(_logInCommand);
        }

        private IDisposable WhenTheUserClicksTheLogInButtonAttemptToLogIn()
        {
            return _logInCommand
                .SelectMany(_ => Observable.CombineLatest(_username, _password, (username, password) => new AuthenticationRequest(username, password)).Take(1))
                .SelectMany(request => _authenticationService.AuthenticateAsync(request))
                .Subscribe(_logInResponse);
        }

        private IDisposable WhenASuccessfulLogInAttemptIsMadeCloseTheDialog()
        {
            return _logInResponse
                .Where(response => response.Successful)
                .Subscribe(response => TryClose(true));
        }

        private IDisposable WhenAnUnsuccessfulLogInAttemptIsMadeDisplayTheError()
        {
            return _logInResponse
                .Where(response => !response.Successful)
                .Select(response => response.Error.Message)
                .Subscribe(_error);
        }

        private IDisposable WhenTheUserClicksTheCancelButtonCloseTheDialog()
        {
            return _cancelCommand
                .Subscribe(_ => TryClose(false));
        }

        public void Dispose()
        {
            if (_behaviors != null)
            {
                _behaviors.Dispose();
                _behaviors = null;
            }
        }

        public string Username
        {
            get { return _username.Get(); }
            set { _username.Set(value); }
        }

        public string Password
        {
            get { return _password.Get(); }
            set { _password.Set(value); }
        }

        public string Error
        {
            get { return _error.Get(); }
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