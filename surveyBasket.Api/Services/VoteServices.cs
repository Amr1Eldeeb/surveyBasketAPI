
using Microsoft.EntityFrameworkCore;
using surveyBasket.Api.Contracts.Questions;
using surveyBasket.Api.Contracts.Votes;

namespace surveyBasket.Api.Services
{
    public class VoteServices(ApplicationDbContext Context) : IVoteServices
    {
        private readonly ApplicationDbContext _context = Context;

        public async Task<Result> AddVoteAsync(int pollId,string userId,voteRequest request, CancellationToken cancellationToken = default)
        {
            //if user have voted on this b same poll or not checking
            var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userId, cancellationToken);


            if (hasVote)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);


            var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId &&
               x.IsPublished == true && x.StartAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

            if (!pollIsExist)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollsErrors.PollNotFound);
            var avaliableQuestion = await _context.Questions.Where(x => x.pollId == pollId && x.IsActive)
                .Select(x=>x.Id )
                .ToListAsync(cancellationToken);
            if(!request.answer.Select(x=>x.questionId).SequenceEqual(avaliableQuestion))
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.InvalidQuestion);

            }
            var Vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                Answers = request.answer.Adapt<IEnumerable<VoteAnswer>>().ToList()
            };
            await _context.Votes.AddAsync(Vote,cancellationToken);    
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
