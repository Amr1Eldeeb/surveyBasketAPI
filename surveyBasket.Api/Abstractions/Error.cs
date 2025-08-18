namespace surveyBasket.Api.Abstractions
{
 
    public record Error(string Code, string ErrorDescription,int? StatusCode )
    {


        public static readonly Error None = new(string.Empty, string.Empty,null );

    }



}
