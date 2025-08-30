using surveyBasket.Api.Contracts.Results;

namespace surveyBasket.Api.Services
{
    public class ResultService(ApplicationDbContext context) : IResultServices
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<PollVotesResponse>> GetPollVoteAsync(int? pollId, CancellationToken cancellationToken = default)
        {
            var pollvotes = await _context.Polls
                .Where(x => x.Id == pollId)
                .Select(x => new PollVotesResponse(
                    x.Title,
                    x.Votes.Select(v => new VoteResponse
                    (
                        $"{v.User.FirstName} {v.User.LastName}",
                        v.SubmittedOn,
                        v.Answers.Select(x => new QuestionAnswerResponse(

                            x.Question.Content,
                            x.Answer.Content

                            ))
                    )
                    )
                    )

                    ).SingleOrDefaultAsync(cancellationToken);


            return pollvotes is null ? Result.Failure<PollVotesResponse>(PollsErrors.PollNotFound) : Result.Success(pollvotes);



        }
        public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayasync(int? pollId, CancellationToken cancellationToken = default)
        {

            var polls = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);

            if (!polls)
            {
                return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollsErrors.PollNotFound);
            }

            var votesperday = await _context.Votes.Where(x => x.Id == pollId)
             .GroupBy(v => new { voteDate = DateOnly.FromDateTime(v.SubmittedOn) })
               .Select(g => new VotesPerDayResponse(
                   g.Key.voteDate,
                   g.Count()

                   )).ToListAsync(cancellationToken);



            return Result.Success<IEnumerable<VotesPerDayResponse>>(votesperday);

        }

        public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionasync(int? pollId, CancellationToken cancellationToken = default)
        {

            var polls = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);

            if (!polls)
            {
                return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollsErrors.PollNotFound);
            }

             var votesperQuestion = await _context.VoteAnswers
                .Where(x=>x.Vote.PollId ==pollId).
                Select(x=> new VotesPerQuestionResponse(
                   
                    x.Question.Content,
                    x.Question.Votes.GroupBy(x=>new {answersId= x.Answer.Id,answerContent = x.Answer.Content })
                    .Select(g=> new VotesPerAnswerResponse
                    ( g.Key.answerContent ,g.Count()) )
                    
                    ))
            .ToListAsync(cancellationToken);    


            return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesperQuestion);

        }

    }
}
