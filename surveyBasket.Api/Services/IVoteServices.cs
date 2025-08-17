using surveyBasket.Api.Contracts.Votes;

namespace surveyBasket.Api.Services
{
    public interface  IVoteServices
    {
        Task<Result> AddVoteAsync( int pollId,Vote vote ,voteRequest request , CancellationToken cancellationToken =default);




    }
}
