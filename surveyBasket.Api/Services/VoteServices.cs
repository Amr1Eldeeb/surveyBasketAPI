
using Microsoft.EntityFrameworkCore;
using surveyBasket.Api.Contracts.Questions;
using surveyBasket.Api.Contracts.Votes;

namespace surveyBasket.Api.Services
{
    public class VoteServices(ApplicationDbContext Context) : IVoteServices
    {
        private readonly ApplicationDbContext _context = Context;

        public async Task<Result> AddVoteAsync(int pollId, Vote vote, voteRequest request, CancellationToken cancellationToken = default)
        {
            //if user have voted on this b same poll or not checking
            var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userID, cancellationToken);


            if (hasVote)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);


            var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId &&
               x.IsPublished == true && x.StartAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

            if (!pollIsExist)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollsErrors.PollNotFound);
             var avaliableQuestion  = await _context.Questions.AnyAsync(

        }
    }
}
