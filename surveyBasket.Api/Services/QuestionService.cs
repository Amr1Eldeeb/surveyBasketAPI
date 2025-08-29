using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using surveyBasket.Api.Contracts.Answers;
using surveyBasket.Api.Contracts.Questions;
using surveyBasket.Api.Entites;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace surveyBasket.Api.Services
{
    public class QuestionService(HybridCache hybridCache
        , ApplicationDbContext context
      
        ) : IQuestionService
    {
        private readonly HybridCache _hybridCache = hybridCache;

        //private readonly ICacheServices _cacheServices = cacheServices;
        private readonly ApplicationDbContext _context = context;
        //private readonly IOutputCacheStore _outputCacheStore = outputCacheStore;
        //private readonly IDistributedCache DistributedCache = distributedCache;
        private const string _cachePrefix = "availableQuestions";
        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
        {

            var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);
            if (!pollIsExists)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(PollsErrors.PollNotFound);
            }
            var question = await _context.Questions.Where(x => x.pollId == pollId)
                .Include(x => x.Answers)
                .Select(q => new QuestionResponse(
                    q.Id,
                    q.Content,
                    q.Answers.Select(a => new AnswerResponse(a.Id, a.Content))
                    ))
                .AsNoTracking()
                .ToListAsync(cancellationToken);//هي هي لو استخدمت mapster 



            return Result.Success<IEnumerable<QuestionResponse>>(question);



        }
        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvaliableAsync(int pollId, string userID, CancellationToken cancellationToken = default)
        {
            var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId &&
              x.IsPublished == true && x.StartAt <= DateOnly.FromDateTime(DateTime.UtcNow)
            && x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);
            //if user have voted on this b same poll or not checking
            var hasVote = await _context.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userID, cancellationToken);

            if (!pollIsExist)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollsErrors.PollNotFound);

            if (hasVote)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);

            var cacheKey = $"{_cachePrefix}-{pollId}";


            var questions = await _hybridCache.GetOrCreateAsync<IEnumerable<QuestionResponse>>(
                cacheKey,
                async cacheEntry => await _context.Questions
                    .Where(x => x.pollId == pollId && x.IsActive)
                    .Include(x => x.Answers)
                    .Select(q => new QuestionResponse(
                        q.Id,
                        q.Content,
                        q.Answers.Where(a => a.IsActive).Select(a => new Contracts.Answers.AnswerResponse(a.Id, a.Content))
                    ))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken)
            );




            return Result.Success<IEnumerable<QuestionResponse>>(questions);

        }
        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {

            var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);
            if (!pollIsExists)
            {
                return Result.Failure<QuestionResponse>(PollsErrors.PollNotFound);
            }
            var QuestionIsExist = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.pollId == pollId, cancellationToken: cancellationToken);

            if (QuestionIsExist)
            {
                return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

            }
            var question = request.Adapt<Question>();
            question.pollId = pollId;
            request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));
            await _context.AddAsync(question, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            //await _outputCacheStore.EvictByTagAsync("AvaliableQuestions", cancellationToken);
            return Result.Success(question.Adapt<QuestionResponse>());

        }

        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {


            var question = await _context.Questions.Where(x => x.pollId == pollId && x.Id == id)
                .Include(x => x.Answers)
                 .ProjectToType<QuestionResponse>()
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);
            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
            return Result.Success(question);
        }

        public async Task<Result> ToggleStatuesAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x => x.Id == id && x.pollId == pollId, cancellationToken);

            if (question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound);
            question.IsActive = !question.IsActive;
            await _context.SaveChangesAsync(cancellationToken);
            //await _outputCacheStore.EvictByTagAsync("AvaliableQuestions", cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
        {

            //accept content if id === id  mean content not changed and not accept if content ==contentr and not same id
            var questionIsExists = await _context.Questions.
            AnyAsync(x => x.pollId == pollId
            && x.Id != id
            && x.Content == request.Content,
            cancellationToken
            );

            if (questionIsExists)
                return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

            var question = await _context.Questions.
                Include(x => x.Answers).SingleOrDefaultAsync(x => x.Id == id && x.pollId == pollId, cancellationToken);

            if (question is null)
                return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

            question.Content = request.Content;
            //current answers 
            var currentAnswers = question.Answers.Select(x => x.Content).ToList();

            //add new answers in db
            var newAnswers = request.Answers.Except(currentAnswers).ToList();

            foreach (var item in newAnswers)
            {
                question.Answers.Add(new Answer { Content = item });
            }// == of newAnswers.foreach(answer=>question.Answers(add(new Asnwer{content =answer})

            question.Answers.ToList().ForEach(answer =>
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            });
            await _context.SaveChangesAsync(cancellationToken);
            //await _outputCacheStore.EvictByTagAsync("AvaliableQuestions", cancellationToken);

            return Result.Success();
        }
    }
}
