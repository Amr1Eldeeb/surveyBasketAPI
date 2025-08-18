namespace surveyBasket.Api.Contracts.Votes
{
    public class voteAnswerRequestValidator:AbstractValidator<VoteAnswerRequest>
    {
        public voteAnswerRequestValidator()
        {
            RuleFor(x => x.questionId).GreaterThan(0);
              
            RuleFor(x => x.answerId).GreaterThan(0);

        }

    }
}
