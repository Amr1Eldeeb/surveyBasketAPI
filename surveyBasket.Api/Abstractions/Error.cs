namespace surveyBasket.Api.Abstractions
{
 
    public record Error(string Code, string ErrorDescription)
    {


        public static readonly Error None = new(string.Empty, string.Empty);

    }



}
