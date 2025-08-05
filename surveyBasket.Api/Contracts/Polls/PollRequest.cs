using surveyBasket.Api.Contracts.Polls;

namespace surveyBasket.Api.Contracts.Polls
{

    public record   PollRequest
        (
        
        string Title,
        string Summary,
        DateOnly StartAt,
        DateOnly EndAt
        
        );



}












//public static implicit operator Poll(CreatePollRequest poll)
//{
//    return new()
//    {
//        Description = poll.Description,
//        Title = poll.Title,
//    };

//}