namespace surveyBasket.Api.Errors
{
    public  static class PollsErrors
    {
        public static readonly Error PollNotFound =
            new Error("Poll.NotFound", "No was Found was found with id  <=> ");
        public static readonly Error DuplcatedPollTitled =
            new Error("Poll Duplcated", "Anthor poll with the same is already Exsist");

    }
}
