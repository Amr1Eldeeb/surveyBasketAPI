﻿namespace surveyBasket.Api.Entites
{
    public sealed class Question :AuditableEntity
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;
        public int pollId { get; set; }
        public bool IsActive { get; set; } = true;
        public Poll poll { get; set; } = default!;
        public ICollection<Answer> Answers { get; set; } = [];
        public ICollection<VoteAnswer> Votes { get; set; } = [];


    }
}
