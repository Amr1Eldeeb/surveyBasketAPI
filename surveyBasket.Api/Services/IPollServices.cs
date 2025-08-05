using System.Threading;

namespace surveyBasket.Api.Services
{
    public interface IPollServices
    {
       Task<Result<IEnumerable<PollResponse>>> GetAll(CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> GetById(int id, CancellationToken cancellationToken = default);
        Task<Result<PollResponse>>Add(PollRequest poll, CancellationToken cancellationToken =default);
        Task<Result> Update(int id, PollRequest poll, CancellationToken cancellationToken = default);
        Task<Result> Delete(int id, CancellationToken cancellationToken = default);
        Task<Result>TogglePublishedStatues(int id, CancellationToken cancellationToken = default);

    }
}
