

namespace surveyBasket.Api.Services
{
    public class PollServices : IPollServices
    {
        private readonly ApplicationDbContext _context;

        public PollServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<IEnumerable<PollResponse>>> GetAll(CancellationToken cancellationToken = default)
        {

            var polls  = await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
            var result = polls.Adapt<IEnumerable<PollResponse>>();

            return Result.Success(result);


        }
        public async Task<Result<PollResponse>> GetById(int id, CancellationToken cancellationToken = default)

        {
            var poll  = await _context.Polls.FindAsync(id,cancellationToken);
            return poll is not null  
                ?Result.Success(poll.Adapt<PollResponse>())
                :Result.Failure<PollResponse>(PollsErrors.PollNotFound);


        }

        public async Task<Result<PollResponse>> Add(PollRequest poll, CancellationToken cancellationToken = default)
        {
            var isExistingTitle = await _context.Polls.AnyAsync(x => x.Title == poll.Title ,cancellationToken:cancellationToken);
            if(isExistingTitle)
            {
                return Result.Failure<PollResponse>(PollsErrors.DuplcatedPollTitled);
            }
           var result =poll.Adapt<Poll>();
            var pollresponse = result.Adapt<PollResponse>();
            await _context.Polls.AddAsync(result, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(pollresponse);
        }
        //ELhamdellah Alzy By Neamth tatem elshlal




        public async Task<Result> Update(int id, PollRequest poll, CancellationToken cancellationToken = default)

        {
            var currentpoll = await _context.Polls.FindAsync(id, cancellationToken);

            var isExistingTitle = await _context.Polls.AnyAsync(x => x.Title == poll.Title &&x.Id!=id, cancellationToken: cancellationToken);
            if (isExistingTitle)
            {
                return Result.Failure<PollResponse>(PollsErrors.DuplcatedPollTitled);
            }



            if (currentpoll is null) 

                return Result.Failure(PollsErrors.PollNotFound);

            currentpoll.Title = poll.Title;
            currentpoll.Summary = poll.Summary;
            currentpoll.StartAt = poll.StartAt;
            currentpoll.EndAt = poll.EndAt;
            ///  currentpoll.IsPublished = poll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        public async Task<Result> Delete(int id, CancellationToken cancellationToken = default)
        {
            var Currentpoll = await _context.Polls.FindAsync(id);
            if (Currentpoll == null)
                return Result.Failure(PollsErrors.PollNotFound);
            _context.Polls.Remove(Currentpoll);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result> TogglePublishedStatues(int id, CancellationToken cancellationToken = default)
        {

            var poll =await _context.Polls.FindAsync(id);
            //var currenPoll = await _context.Polls.SingleOrDefaultAsync(x=>x.Id == id);
            if (poll is null) 
                return Result.Failure(PollsErrors.PollNotFound);
            poll.IsPublished = !poll.IsPublished;

            ///  currentpoll.IsPublished = poll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();



        }
      public async Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default)
      {
           var polls =  await  _context.Polls.Where(x=>x.IsPublished ==true && x.StartAt<= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow)).
                AsNoTracking().ProjectToType<PollResponse>().ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<PollResponse>>(polls);

        }

    }
}
