using surveyBasket.Api.Contracts.Votes;

namespace surveyBasket.Api.Services
{
    public interface  IVoteServices
    {
        Task<Result> AddVoteAsync( int pollId,string userId ,voteRequest request , CancellationToken cancellationToken =default);




    }
}
