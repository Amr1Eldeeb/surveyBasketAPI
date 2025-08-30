

namespace surveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser>userManager,
        SignInManager<ApplicationUser>signInManager,
        IJwtProvider _jwtProvider) : IAuthService
    {
        public UserManager<ApplicationUser> UserManager  = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
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
            var result =await _signInManager.PasswordSignInAsync(user,password,false,false);

             if(result.Succeeded)
            {
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
                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresin, RefreshToken, RefreshTokenExpiration);
                //return Result.Success<AuthResponse>(response); 
                //or 
                return Result.Success(response);

            }
            return Result.Failure<AuthResponse>(UserErrors.EmailConfirmed);

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
            var result = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, Newtoken, expiresin, NewRefreshToken, RefreshTokenExpiration);
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

        //auto sign in after register
        public async Task<Result<AuthResponse>>RegisterAsync(RegisterRequest request , CancellationToken cancellationToken =default )
        {
            var EmailIsExists = await UserManager.Users.AnyAsync(x=>x.Email == request.Email,cancellationToken);
            if (EmailIsExists)  
                return Result.Failure<AuthResponse>(UserErrors.DuplicatedEmail);

            //var user = new ApplicationUser
            //{
            //    Email = request.Email,
            //    UserName = request.Email,
            //    Firstname =request.FirstName,
            //    LastName = request.LastName,

            //}; هي هي adapt
            var user  = request.Adapt<ApplicationUser>();    
            
            var result = await UserManager.CreateAsync(user,request.Password);
            if (result.Succeeded)
            {
                var (token, expiresin) = JwtProvider.GenerateToken(user);

                var RefreshToken = GenerateRefreshToken();

                var RefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

    
                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = RefreshToken,
                    ExpiresOn = RefreshTokenExpiration


                });

                await UserManager.UpdateAsync(user);
                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresin, RefreshToken, RefreshTokenExpiration);
                //return Result.Success<AuthResponse>(response); 
                //or 
                return Result.Success(response);
            }

            var error = result.Errors.First();
            return Result.Failure<AuthResponse>
                (new Error(error.Code ,error.Description,StatusCodes.Status408RequestTimeout));

        }





    }
}
