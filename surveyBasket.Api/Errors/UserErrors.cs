using surveyBasket.Api.Abstractions;

namespace surveyBasket.Api.Errors
{
    public static class UserErrors
    {

        //Create  Class for any entity to read from them error dsexripation
        public static readonly Error InvalidCredentials =
            new Error("User.InvalidCredentials", "Invalid Email or Password");





    }
}
