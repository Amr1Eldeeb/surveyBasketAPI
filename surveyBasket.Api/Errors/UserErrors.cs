using surveyBasket.Api.Abstractions;

namespace surveyBasket.Api.Errors
{
    public static class UserErrors
    {

        //Create  Class for any entity to read from them error dsexripation
        public static readonly Error InvalidCredentials =
            new Error("User.InvalidCredentials", "Invalid Email or Password", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidJWtToken =
    new Error("User.InvalidJWTToken", "Invalid JWt  Token", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidResfreshToken =
    new Error("User.InvalidResfreshToken", "Invalid Fresfresh Token  Token", StatusCodes.Status401Unauthorized);

    }
}
