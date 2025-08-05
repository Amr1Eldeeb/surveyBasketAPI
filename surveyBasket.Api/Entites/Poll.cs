using surveyBasket.Api.Contracts.Polls;

namespace surveyBasket.Api.Entites
{
    public class Poll : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
         public string Summary { get; set; } = string.Empty;
        public bool IsPublished {  get; set; }
        public DateOnly StartAt { get; set; }
        public DateOnly EndAt { get; set; }
    

    }
}






















        //public static implicit operator PollResponse (Poll poll)
        //{
        //    return new()
        //    {
        //        Id = poll.Id,
        //        Description = poll.Description,
        //        Title = poll.Title,
        //    };

        //}