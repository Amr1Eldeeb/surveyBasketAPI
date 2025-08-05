using Microsoft.AspNetCore.Identity;

namespace surveyBasket.Api.Entites
{
    public class ApplicationUser:IdentityUser
    {

        public string Firstname { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public List<RefreshToken> RefreshTokens { get; set; } = [];


    }
}
