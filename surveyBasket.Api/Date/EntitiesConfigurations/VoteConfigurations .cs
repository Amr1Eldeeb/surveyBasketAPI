
namespace surveyBasket.Api.Date.EntitiesConfigurations
{
    public class VoteConfigurations : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(e => new { e.Id, e.UserId }).IsUnique();
           
        }
    }
}
