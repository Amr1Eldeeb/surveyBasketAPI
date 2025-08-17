namespace surveyBasket.Api.Errors
{
    public  static class VoteErrors
    {
        public static readonly Error DuplicatedVote=
            new Error("Vote.Duplicated", "This User Already has Voted ");
  

    }
}
