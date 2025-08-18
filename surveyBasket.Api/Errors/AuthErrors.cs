namespace surveyBasket.Api.Errors
{
    public static class AuthErrors
    {
        public static readonly Error AuthErrorMassage =
       new Error("Email Or Password Is wrong", " Plz Try Again ", StatusCodes.Status404NotFound);
    }
}
