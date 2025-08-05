namespace surveyBasket.Api.Contracts.Authencation
{
    public class LoginRequestValidator : AbstractValidator<Login>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
     RuleFor(x => x.Password).NotEmpty();

        }
      
    }
}
