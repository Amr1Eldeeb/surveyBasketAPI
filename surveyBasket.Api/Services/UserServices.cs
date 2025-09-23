using surveyBasket.Api.Contracts.Users;

namespace surveyBasket.Api.Services
{
    public class UserServices(UserManager<ApplicationUser>user) : IUserServices
    {
        public UserManager<ApplicationUser> _userManger  = user;


        public async Task<Result<UserProfileResponse>>GetProfileAsync(string userId)
        {
             var user =await  _userManger.Users.Where(x=>x.Id == userId)
                .ProjectToType<UserProfileResponse>().SingleAsync();

            return Result.Success(user);
        }
        public async Task<Result>UpdateProfileAsync(string Id ,UpdateProfileRequest request)
        {
            // var user = await _userManger.FindByIdAsync(Id);

            // user = request.Adapt(user);
            //await _userManger.UpdateAsync(user!);
            await _userManger.Users.Where(x => x.Id == Id)
                 .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.FirstName, request.FirstName)
                .SetProperty(x => x.LastName, request.LastName)
                );
            return Result.Success();
        }
        public async Task<Result> ChangePassword(string userId, ChangePasswordRequest request )
        {
            var user = await _userManger.FindByIdAsync(userId);
            var result =await _userManger.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);
            if(result.Succeeded)
            {
                return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }

    }
}
