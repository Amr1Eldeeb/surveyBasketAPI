using surveyBasket.Api.Contracts.Results;

namespace surveyBasket.Api.Services
{
    public interface IResultServices
    {
        public  Task<Result<PollVotesResponse>> GetPollVoteAsync(int? pollId, CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayasync(int? pollId, CancellationToken cancellationToken = default);

        public Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionasync(int? pollId, CancellationToken cancellationToken = default);

    }
}
