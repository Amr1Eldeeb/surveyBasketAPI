
using surveyBasket.Api.Abstractions.Consts;

namespace surveyBasket.Api.Contracts.Users
{
    public class ChangePasswordRequestValidator:AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty()
               .Matches(RegexPatterns.Password)
               .WithMessage("Password should be at least 8 digits and should contains LowerCase and NonAlphabetic")
               .MinimumLength(8).NotEqual(x=>x.CurrentPassword).
               WithMessage("new password cannot same as the current password");

            RuleFor(x => x.CurrentPassword).
                NotEmpty();
        }
    }
}
