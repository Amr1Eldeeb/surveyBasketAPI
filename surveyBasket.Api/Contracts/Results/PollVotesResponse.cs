namespace surveyBasket.Api.Contracts.Results
{
    public record PollVotesResponse
        (
        string Title,// of poll  
        IEnumerable<VoteResponse>votes // list of Votes
        
        
        
        );
    
}
