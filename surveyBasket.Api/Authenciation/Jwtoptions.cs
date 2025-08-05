namespace surveyBasket.Api.Authenciation
{
    public class JwtOptions
    {
        public static string SectionName = "Jwt"; // for part of sections
        public string key { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiryMinutes { get; init; }  


    }
}
