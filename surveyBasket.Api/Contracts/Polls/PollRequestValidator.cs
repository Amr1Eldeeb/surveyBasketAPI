namespace surveyBasket.Api.Contracts.Polls
{
    public class PollRequestValidator:AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(prop => prop.Title)
                .NotEmpty()
                .Length(3,40)
                .WithMessage("Plz add a {PropertyName}")
                .WithMessage("Please enter a valid value you entered {PropertyValue}");
            //==NotEmpty
            RuleFor(prop => prop.Summary)
                .NotEmpty().Length(5, 30).
                WithMessage("Plz enter a valid desription");
            RuleFor(x => x.StartAt)
                .NotEmpty().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(x => x.EndAt)
                .NotEmpty();
            RuleFor(x => x)
                .Must(HasvalidDate).WithName(nameof(PollRequest.EndAt)) // عشان نعين اسم لان المست علي الكلاس كله
                // for handel propname Because must for all class and not defined what is the prop has contrioeld
                .WithMessage("plz enter a valid date {PropertyName}");

        }
        private bool HasvalidDate(PollRequest pollrequest)
            //custom validator and add it to ruleFor using must for a all class
        {
            return pollrequest.EndAt >= pollrequest.StartAt;
        }
    }
}
