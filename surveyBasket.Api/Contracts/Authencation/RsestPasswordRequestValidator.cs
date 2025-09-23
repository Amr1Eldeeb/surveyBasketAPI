using surveyBasket.Api.Abstractions.Consts;

namespace surveyBasket.Api.Contracts.Authencation
{
    public class RsestPasswordRequestValidator:AbstractValidator<RsestPasswordRequest>
    {

        public RsestPasswordRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();

            RuleFor(x => x.NewPassword).NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains LowerCase and NonAlphabetic")
                .MinimumLength(8);
            RuleFor(x => x.code).NotEmpty();
        }
    }
}
