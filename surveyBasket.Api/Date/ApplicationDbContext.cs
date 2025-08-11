
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using surveyBasket.Api.Date.EntitiesConfigurations;
using System;
using System.Security.Claims;

namespace surveyBasket.Api.Date
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor)
        : IdentityDbContext<ApplicationUser>(options) 
    {
        private readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;

        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }   
        public DbSet<Answer> Answers {  get; set; } 
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteAnswer>VoteAnswers { get; set; }   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Poll>().Property(x => x.Title).
                HasMaxLength(100);*/
          //  modelBuilder.ApplyConfiguration(new PollConfigurations());// For Each one entites
          modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(), // هنا بيتولد تلقائيًا
                UserName = "admin",
                Firstname = "Amr",
                LastName = "khaled",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
            };
            user.PasswordHash = hasher.HashPassword(user, "Admin@123");

            modelBuilder.Entity<ApplicationUser>().HasData(user);
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk=>fk.DeleteBehavior == DeleteBehavior.Cascade
                &&!fk.IsOwnership);


            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            }





            base.OnModelCreating(modelBuilder);
        }

        // it return number of rows has effected in db
        public override Task<int> SaveChangesAsync( CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            //معاناها اي حد هيورث الكلاس دا هيطبق عليه التغيررات
            foreach (var entry in entries)
            {
                var currentUserID  = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!; //id
                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedById).CurrentValue = currentUserID!;

                } 
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedById).CurrentValue = currentUserID;
                    entry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow; 

                }

            }

            return base.SaveChangesAsync(cancellationToken);
        }









    }

}



















