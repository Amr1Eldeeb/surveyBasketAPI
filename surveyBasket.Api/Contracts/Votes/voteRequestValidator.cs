namespace surveyBasket.Api.Contracts.Votes
{
    public class voteRequestValidator:AbstractValidator<voteRequest>
    {
        public voteRequestValidator()
        {
            RuleFor(x => x.answer).NotEmpty();

        }

    }
}
