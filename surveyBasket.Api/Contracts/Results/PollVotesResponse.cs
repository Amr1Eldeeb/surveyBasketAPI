namespace surveyBasket.Api.Contracts.Results
{
    public record PollVotesResponse
        (
        string Title,
        IEnumerable<VoteResponse>votes
        
        
        
        );
    
}
