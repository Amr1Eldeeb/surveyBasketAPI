

namespace surveyBasket.Api.Date.EntitiesConfigurations
{
    public class PollConfigurations : IEntityTypeConfiguration<Poll>
    { 
        // for validation  
        public void Configure(EntityTypeBuilder<Poll> builder) //دي ميثود بتبطفقها
        {
            builder.HasIndex(x=>x.Title).IsUnique();
            builder.Property(x=>x.Title).HasMaxLength(1500);
            builder.Property(x => x.Summary).HasMaxLength(1500);
        }
    }
}
