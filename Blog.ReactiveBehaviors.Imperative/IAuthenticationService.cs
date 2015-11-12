using System.Threading;
using System.Threading.Tasks;

namespace Blog.ReactiveBehaviors.Imperative
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest, CancellationToken token);
    }
}