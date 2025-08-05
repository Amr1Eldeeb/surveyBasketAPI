namespace surveyBasket.Api.Authenciation
{
    public interface IJwtProvider
    {

        (string token, int expiresIn) GenerateToken(ApplicationUser user);
        
        //it s tuple
        string? ValidateToken (string token);


    }
}
