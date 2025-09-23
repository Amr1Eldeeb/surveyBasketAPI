using surveyBasket.Api.Contracts.Users;
using System.Globalization;

namespace surveyBasket.Api.Services
{
    public interface IUserServices
    {
        public Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
        public Task<Result> UpdateProfileAsync(string Id, UpdateProfileRequest request);
        public Task<Result> ChangePassword(string userId ,ChangePasswordRequest request);
    }
}
