namespace surveyBasket.Api.Services
{
    public interface ICacheServices
    {
        Task<T?>GetAsync<T>(string key ,CancellationToken cancellationToken = default) where T :class;
        Task SetAsync<T>(string key , T value ,CancellationToken cancellationToken = default) where T :class;
        Task Remove(string key ,CancellationToken cancellationToken = default);


    }
}
