namespace surveyBasket.Api.Errors
{
    public  static class QuestionErrors
    {
        public static readonly Error QuestionNotFound =
            new Error("Question.NotFound", "No Question Found was  the give  id  <=> ", StatusCodes.Status404NotFound);
        public static readonly Error DuplicatedQuestionContent =
           new Error("Question.Duplicated", "The Question Is Duplicated Broo <=> ", StatusCodes.Status409Conflict);
    }
}
