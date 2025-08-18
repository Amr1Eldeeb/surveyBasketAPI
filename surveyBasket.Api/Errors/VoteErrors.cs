namespace surveyBasket.Api.Errors
{
    public  static class VoteErrors
    {
        public static readonly Error DuplicatedVote =
            new Error("Vote.Duplicated", "This User Already has Voted ", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidQuestion =
                new Error("Vote.InvalidQuestions", "Invalid Question ",StatusCodes.Status409Conflict);


    }
}
