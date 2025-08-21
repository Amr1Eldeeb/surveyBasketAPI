namespace surveyBasket.Api.Contracts.Results
{
    public record VotesPerQuestionResponse
   (string Question,
        IEnumerable<VotesPerAnswerResponse> SelecteAnswwer);
}
