namespace surveyBasket.Api.Services
{
    public interface INotificationsServices
    {
        Task SnedNewPollsNotifications(int? pollId =null);
    }
}
