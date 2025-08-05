

namespace surveyBasket.Api.Date.EntitiesConfigurations
{
    public class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    { 
        // for validation  
        public void Configure(EntityTypeBuilder<ApplicationUser> builder) //دي ميثود بتبطفقها
        {
            builder.Property(x => x.Firstname).HasMaxLength(100);
            builder.Property(x=>x.LastName).HasMaxLength(100);
            builder.OwnsMany(x => x.RefreshTokens)
                .ToTable("RefreshTokens") //to change name of Table 
                .WithOwner()
                .HasForeignKey("UserId");
        }
    }
}
