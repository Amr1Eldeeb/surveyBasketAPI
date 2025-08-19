using surveyBasket.Api.Contracts.Results;

namespace surveyBasket.Api.Services
{
    public class ResultService(ApplicationDbContext context) :IResultServices
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<PollVotesResponse>>GetPollVoteAsync(int? pollId ,CancellationToken cancellationToken =  default)
        {
            var pollvotes = await _context.Polls
                .Where(x=>x.Id == pollId)
                .Select(x => new PollVotesResponse(
                    x.Title,
                    x.Votes.Select(v => new VoteResponse
                    (
                        $"{v.User.Firstname} {v.User.LastName}",
                        v.SubmittedOn,
                        v.Answers.Select(x => new QuestionAnswerResponse(

                            x.Question.Content,
                            x.Answer.Content

                            ))
                    )
                    )
                    )

                    ).SingleOrDefaultAsync(cancellationToken);


            return pollvotes is null ?Result.Failure<PollVotesResponse>(PollsErrors.PollNotFound):Result.Success(pollvotes);



        }





    }
}
