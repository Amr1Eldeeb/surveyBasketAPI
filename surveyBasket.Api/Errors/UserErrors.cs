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
    
        public static readonly Error DuplicatedEmail =
    new Error("User.DuplicatedEmail", "Another user with the same email is already Exists", StatusCodes.Status409Conflict);

        public static readonly Error EmailNotConfirmed =
    new Error("User.Email NotConfirmed", " The Email is not Confirmed ", StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidCode =
    new Error("User.InvalidCode", " The Code is Invalid ", StatusCodes.Status401Unauthorized);
        public static readonly Error DuplicatedConfirmation =
new Error("User.Duplicated Confirmation", " Email Already Confirmed ", StatusCodes.Status401Unauthorized);
    }
}
