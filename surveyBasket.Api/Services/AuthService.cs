

namespace surveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser>userManager,IJwtProvider _jwtProvider) : IAuthService
    {
        public UserManager<ApplicationUser> UserManager  = userManager;

        private IJwtProvider JwtProvider  = _jwtProvider;
        private readonly int _refreshTokenExpiryDays = 14;
        public async Task<Result<AuthResponse>> getTokenasync(string Email, string password, CancellationToken cancellationToken = default)
        {
            //check User? is finded
            var user = await UserManager.FindByEmailAsync(Email); // its nullable

            if(user is null ) 
            {
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials); 
            }

            //check Password
           var IsValidPassword =  await UserManager.CheckPasswordAsync(user, password);
            if(!IsValidPassword) 
            {
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            }
            //generate jwt
            //return new Authservice 
            var (token, expiresin) = JwtProvider.GenerateToken(user);

            var RefreshToken = GenerateRefreshToken();

            var RefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);


            user.RefreshTokens.Add(new RefreshToken
            {
                Token = RefreshToken,
                ExpiresOn = RefreshTokenExpiration


            });

            await UserManager.UpdateAsync(user);

            //return new AuthResponse(user.Id,user.Email,user.Firstname,user.LastName,token,expiresin,RefreshToken,RefreshTokenExpiration);   
            var response = new AuthResponse(user.Id, user.Email, user.Firstname, user.LastName, token, expiresin, RefreshToken, RefreshTokenExpiration);
            //return Result.Success<AuthResponse>(response); 
            //or 
            return Result.Success(response);


        }

        public async Task<Result<AuthResponse?>> getRefreshToken(string oldToken, string resfreshtoken, CancellationToken cancellationToken = default)
        {

            var UserId =  JwtProvider.ValidateToken(oldToken);

            if (UserId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials)!;

            var user  = await UserManager.FindByIdAsync(UserId);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials)!;


            var userfrefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == resfreshtoken && x.IsActive);
            if (userfrefreshToken is null)
                return  Result.Failure<AuthResponse>(UserErrors.InvalidCredentials)!;
            userfrefreshToken.RevokedOn  = DateTime.UtcNow;

            var (Newtoken, expiresin) = JwtProvider.GenerateToken(user);

            var NewRefreshToken = GenerateRefreshToken();

            var RefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);


            user.RefreshTokens.Add(new RefreshToken
            {
                Token = NewRefreshToken,
                ExpiresOn = RefreshTokenExpiration


            });

            await UserManager.UpdateAsync(user);
            var result = new AuthResponse(user.Id, user.Email, user.Firstname, user.LastName, Newtoken, expiresin, NewRefreshToken, RefreshTokenExpiration);
            return Result.Success(result)!;

        }
        public async Task<Result> RevokerefreshToken(string oldToken, string resfreshtoken, CancellationToken cancellationToken = default)
        {
            var UserId = JwtProvider.ValidateToken(oldToken);

            if (UserId is null)
                return Result.Failure(UserErrors.InvalidCredentials);

            var user = await UserManager.FindByIdAsync(UserId);
            if (user is null)
                return Result.Failure(UserErrors.InvalidCredentials);



            var userfrefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == resfreshtoken && x.IsActive);
            
            if (userfrefreshToken is null)
                   return Result.Failure(UserErrors.InvalidCredentials);;


            userfrefreshToken.RevokedOn = DateTime.UtcNow;

            await UserManager.UpdateAsync(user);

            return Result.Success();



        }






        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));




        }

    }
}
