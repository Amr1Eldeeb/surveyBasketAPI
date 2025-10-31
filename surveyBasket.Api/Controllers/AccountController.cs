 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using surveyBasket.Api.Contracts.Users;

namespace surveyBasket.Api.Controllers
{
    [Route("me")]//for standeredd
    [ApiController]
    [Authorize]
    public class AccountController(IUserServices userServices) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;
        [HttpGet("")]
        public async Task<IActionResult>Info()
        {
            var result = await _userServices.GetProfileAsync(User.GetUserId()!);
            return Ok(result.value);
        }
        [HttpPut("info")]
        public async Task<IActionResult> Info([FromBody]UpdateProfileRequest request)
        {
             await _userServices.UpdateProfileAsync(User.GetUserId()!,request);
            return Ok();
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> changePassword([FromBody] ChangePasswordRequest request)
        {
           var result =  await _userServices.ChangePassword(User.GetUserId()!, request);
            return  result.IsSuccess ?NoContent() : result.ToProblem();
        }
    }
}
