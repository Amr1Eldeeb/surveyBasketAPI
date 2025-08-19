namespace surveyBasket.Api.Contracts.Results
{
    public record VoteResponse
        (
        
        string VoterName,
        DateTime Votedate,
        IEnumerable<QuestionAnswerResponse>SelectedAnswers
        
        
        );
   
}
