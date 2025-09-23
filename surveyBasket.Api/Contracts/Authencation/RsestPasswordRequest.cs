namespace surveyBasket.Api.Contracts.Authencation
{
    public record RsestPasswordRequest
   (
        string Email,
        string code ,
        string NewPassword
        );
}
