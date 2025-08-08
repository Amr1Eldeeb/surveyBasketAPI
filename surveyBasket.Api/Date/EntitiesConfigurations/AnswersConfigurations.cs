
namespace surveyBasket.Api.Date.EntitiesConfigurations
{
    public class AnswersConfigurations : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {

            builder.Property(x => x.Content).HasMaxLength(100);
            builder.HasIndex(x => new {x.QuestionId, x.Content}).IsUnique();
            //no duplacte
        }
    }
}
