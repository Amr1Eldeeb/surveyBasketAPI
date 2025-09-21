
using Microsoft.AspNetCore.Identity.UI.Services;
using surveyBasket.Api.Helper;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace surveyBasket.Api.Services
{
    public class NotificationServices(ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager
        ,  IHttpContextAccessor httpContext
        ,
        IEmailSender emailSender) : INotificationsServices
    {
        private readonly ApplicationDbContext _Context = dbContext;
        private readonly UserManager<ApplicationUser> _userManger = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContext;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task SnedNewPollsNotifications(int? pollId = null)
        {
            IEnumerable<Poll> polls = [];
            if (pollId.HasValue)
            {
                var poll = await _Context.Polls.SingleOrDefaultAsync(x => x.Id == pollId && x.IsPublished);
                polls = [poll!];
            }
            else
            {
                polls = await _Context.Polls.Where(x => x.IsPublished && x.StartAt == DateOnly.FromDateTime(DateTime.UtcNow))
                    .AsNoTracking().ToListAsync();


            }
            //TODO select Member Only
            var users = await _userManger.Users.ToListAsync();
            var origan = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            foreach (var poll in polls)
            {
                foreach (var user in users)
                {
                    var placeHolder = new Dictionary<string, string>()
                    {
                        {"{{name}}",user.FirstName },
                        {"{{pollTill}}",poll.Title },
                        {"{{endDate}}",poll.EndAt.ToString() },
                        {"{{url}}", $"{origan}/polls/start/{pollId}"},
                    };
                    var body = EmailBodyHelper.GenerateEmailBody("PollNotification", placeHolder);
                    await _emailSender.SendEmailAsync(user.Email!, $"👿 Survey basket : New Poll - {poll.Title} ✅", body);
                }

            }
        }
    }
}
