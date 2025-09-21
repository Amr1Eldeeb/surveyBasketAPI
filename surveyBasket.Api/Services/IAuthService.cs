
namespace surveyBasket.Api.Services
{
    public interface IAuthService
    {
        //for create JWt 
        Task<Result<AuthResponse>> getTokenasync(string Email, string password,CancellationToken cancellationToken = default);
        Task<Result<AuthResponse?>> getRefreshToken(string oldToken,string resfreshtoken,CancellationToken cancellationToken = default);
        Task<Result> RevokerefreshToken(string oldToken,string resfreshtoken,CancellationToken cancellationToken = default);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        public  Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
    }
}
    