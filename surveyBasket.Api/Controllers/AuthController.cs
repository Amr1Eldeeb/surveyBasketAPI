

using Azure;

namespace surveyBasket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, IOptions<JwtOptions> jwtoptions) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        private readonly JwtOptions _Jwtoptions = jwtoptions.Value;
    
        [HttpPost("")]
        public async Task<IActionResult>Login([FromBody]Login loginRequest, CancellationToken cancellationToken = default)
        {
            //throw new Exception("non e");
            var authResult = await _authService.getTokenasync(loginRequest.Email,
                loginRequest.Password , cancellationToken);

            return authResult.IsSuccess
                ? Ok(authResult.value) :
                authResult.ToProblem(StatusCodes.Status400BadRequest  );
              
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

    }
}
