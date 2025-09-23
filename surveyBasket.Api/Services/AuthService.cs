

using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using surveyBasket.Api.Helper;
using System.Text;

namespace surveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider _jwtProvider, ILogger<AuthService> _logger,
        IEmailSender email,IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        public UserManager<ApplicationUser> UserManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private IJwtProvider JwtProvider = _jwtProvider;
        private readonly ILogger<AuthService> _logger = _logger;
        private readonly int _refreshTokenExpiryDays = 14;
        private readonly IEmailSender _emailSender = email;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<AuthResponse>> getTokenasync(string Email, string password, CancellationToken cancellationToken = default)
        {
            //check User? is finded

            if (await UserManager.FindByEmailAsync(Email) is not { } user) // {} Mean Null
            {
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            }
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!await UserManager.IsEmailConfirmedAsync(user))
            {
                return Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed);
            }
            if (result.Succeeded)
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
            return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);

        }
        public async Task<Result<AuthResponse?>> getRefreshToken(string oldToken, string resfreshtoken, CancellationToken cancellationToken = default)
        {

            var UserId = JwtProvider.ValidateToken(oldToken);

            if (UserId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials)!;

            var user = await UserManager.FindByIdAsync(UserId);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials)!;


            var userfrefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == resfreshtoken && x.IsActive);
            if (userfrefreshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials)!;
            userfrefreshToken.RevokedOn = DateTime.UtcNow;

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
                return Result.Failure(UserErrors.InvalidCredentials); ;


            userfrefreshToken.RevokedOn = DateTime.UtcNow;

            await UserManager.UpdateAsync(user);

            return Result.Success();



        }
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));




        }

        //auto sign in after register
        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var EmailIsExists = await UserManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
            if (EmailIsExists)
                return Result.Failure(UserErrors.DuplicatedEmail);

            var user = request.Adapt<ApplicationUser>();

            var result = await UserManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                //Generete Codee to Send to User to haver On
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user); //buit in in usermnager 

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)); //add it in URl

                _logger.LogInformation("Confirmation Code : {code}", code);
                //TODO  send email 
                var origan = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
                var emailBody = EmailBodyHelper.GenerateEmailBody("Emailconfirmation",
                    new Dictionary<string, string>
                    {
                        {"{{name}}" ,user.FirstName},
                        {"{{action_url}}",$"{origan}/auth/confirm-email?UserId={user.Id}&Code={code}" }
                    }
                    );

                BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "SurveyBasket : EmailConfirmation ✅", emailBody));
                //await _emailSender.SendEmailAsync(user.Email!,"SurveyBasket : EmailConfirmation ✅",emailBody);
                await Task.CompletedTask;
                return Result.Success();

            }

            var error = result.Errors.First();
            return Result.Failure
                (new Error(error.Code, error.Description, StatusCodes.Status408RequestTimeout));

        }
        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            //يفضل منعرفش اليوزر اي ال غلطط هو كدا معروفه ان اليوزر هو ال مش موجود
            if (await UserManager.FindByIdAsync(request.UserId) is not { } user)

                return Result.Failure(UserErrors.InvalidCode);


            if (user.EmailConfirmed)

                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = request.Code;
            try
            {//نرجع الكود لاصله
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)); //for decoding to get code in nutural temp
            }
            catch (FormatException)
            {
                return Result.Failure(UserErrors.InvalidCode);

            }
            var result = await UserManager.ConfirmEmailAsync(user, code);//to confirmCode




            if (result.Succeeded)
            {

                return Result.Success();

            }

            var error = result.Errors.First();
            return Result.Failure
                (new Error(error.Code, error.Description, StatusCodes.Status408RequestTimeout));

        }
        public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
        {
            if (await UserManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();//مش هنبعت اي حاجعه عشان نضلله

            if (user.EmailConfirmed)

                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)); //add it in URl and encoding

            _logger.LogInformation("Confirmation Code : {code}", code);
             
            var origan = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var emailBody = EmailBodyHelper.GenerateEmailBody("Emailconfirmation",
                new Dictionary<string, string>
                {
                        {"{{name}}" ,user.FirstName},
                        {"{{action_url}}",$"{origan}/auth/confirm-email?UserId={user.Id}&code={code}" }
                }
                );

            await _emailSender.SendEmailAsync(user.Email!, "SurveyBasket : EmailConfirmation ✅", emailBody);
            return Result.Success();



        }
        public async Task<Result> SendResetPasswordCodeAsync(string email)
        {
            if(await UserManager.FindByEmailAsync(email) is not { } user)
                return Result.Success();
            if(!user.EmailConfirmed)
            {
                return Result.Failure(UserErrors.EmailNotConfirmed);
            }
            var code = await UserManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)); //add it in URl and encoding

            _logger.LogInformation("Reset  Code : {code}", code);



            await SendResetPasswrodEmail(user,email);
            return Result.Success();


        }
        public async Task<Result>ResetPassswordAsync(RsestPasswordRequest request)
        {
            var user = await UserManager.FindByEmailAsync(request.Email);
            if(user is null ||!user.EmailConfirmed )
            {
                return Result.Failure(UserErrors.InvalidCode);

            }
            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.code));

                result =await UserManager.ResetPasswordAsync(user,code , request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(UserManager.ErrorDescriber.InvalidToken());
            }
            if(result.Succeeded)
            {
                return Result.Success();
            }

            var error = result.Errors.First();
            return Result.Failure
                (new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
        }
        private async Task SendResetPasswrodEmail(ApplicationUser user ,string code)
        {
            var origan = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var emailBody = EmailBodyHelper.GenerateEmailBody("ForgetPassword",
                new Dictionary<string, string>
                {
                        {"{{name}}" ,user.FirstName},
                        {"{{action_url}}",$"{origan}/auth/forget-password?Email={user.Email}&code={code}" }
                }
                );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "SurveyBasket : Change Password ✅", emailBody));
            await Task.CompletedTask;
        }

    }
}
