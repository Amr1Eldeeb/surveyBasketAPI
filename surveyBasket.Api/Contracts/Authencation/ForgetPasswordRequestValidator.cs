namespace surveyBasket.Api.Contracts.Authencation
{
    public class ForgetPasswordRequestValidator:AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
