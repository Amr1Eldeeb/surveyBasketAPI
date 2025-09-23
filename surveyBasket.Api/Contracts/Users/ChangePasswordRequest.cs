namespace surveyBasket.Api.Contracts.Users
{
    public record ChangePasswordRequest
        (
        string CurrentPassword,
        string NewPassword
        );
}

