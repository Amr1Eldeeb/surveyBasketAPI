

using Azure;

namespace surveyBasket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, IOptions<JwtOptions> jwtoptions,ILogger<AuthController>_loggoer) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> loggoer = _loggoer;
        private readonly JwtOptions _Jwtoptions = jwtoptions.Value;
    
        [HttpPost("")]
        public async Task<IActionResult>Login([FromBody]Login loginRequest, CancellationToken cancellationToken = default)
        {

             loggoer.LogInformation("Logging with Email :{email} and password:{password}" ,loginRequest.Email , loginRequest.Password);
            var authResult = await _authService.getTokenasync(loginRequest.Email,
                loginRequest.Password , cancellationToken);

            return authResult.IsSuccess
                ? Ok(authResult.value) :
                authResult.ToProblem();
              
            // standered RFC without propblem

        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {


            var AuthResult = await _authService.getRefreshToken(request.token, request.refreshToken, cancellationToken);

            return AuthResult is null ? BadRequest("Invalid Token") : Ok(AuthResult);

        }


        [HttpPost("revoked-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {


            var IsRevoked = await _authService.RevokerefreshToken(request.token, request.refreshToken, cancellationToken);

            if(IsRevoked.IsSuccess)
            {
                return Ok();
            }
            else
            {
               return  BadRequest("Operation Faild");
            }

        }
        [HttpPost("register")]
        public async Task<IActionResult>Register(RegisterRequest request ,CancellationToken cancellationToken)
        {
            var authResult = await _authService.RegisterAsync(request,cancellationToken);
            return authResult.IsSuccess ? Ok() :authResult.ToProblem();

        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.ConfirmEmailAsync(request);
            return authResult.IsSuccess ? Ok() : authResult.ToProblem();

        }
        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.ResendConfirmationEmailAsync(request);
            return authResult.IsSuccess ? Ok() : authResult.ToProblem();

        }














    }
}
