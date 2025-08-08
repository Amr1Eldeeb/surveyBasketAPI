using surveyBasket.Api.Contracts.Answers;

namespace surveyBasket.Api.Contracts.Questions
{
    public record QuestionResponse
    (
        
        int Id,
        string Content,
        IEnumerable <AnswerResponse>Answers
        




        );
    
}
