using surveyBasket.Api.Contracts.Results;

namespace surveyBasket.Api.Services
{
    public interface IResultServices
    {
        public  Task<Result<PollVotesResponse>> GetPollVoteAsync(int? pollId, CancellationToken cancellationToken = default);



    }
}
