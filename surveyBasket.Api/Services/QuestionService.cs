using surveyBasket.Api.Contracts.Answers;
using surveyBasket.Api.Contracts.Questions;
using surveyBasket.Api.Entites;
using System.Runtime.CompilerServices;

namespace surveyBasket.Api.Services
{
    public class QuestionService(ApplicationDbContext context) : IQuestionService
    {
        private readonly ApplicationDbContext _context = context;
        
        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId,CancellationToken cancellationToken = default)
        {

            var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);
            if (!pollIsExists)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(PollsErrors.PollNotFound);
            }
            var question = await _context.Questions.Where(x => x.pollId == pollId)
                .Include(x => x.Answers)
                .Select(q=> new QuestionResponse(
                    q.Id,
                    q.Content,
                    q.Answers.Select(a=>new AnswerResponse(a.Id,a.Content))  
                    ))
                .AsNoTracking()
                .ToListAsync(cancellationToken);//هي هي لو استخدمت mapster 



            return Result.Success<IEnumerable<QuestionResponse>>(question);



        }
        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {

            var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken:cancellationToken);
            if(!pollIsExists)
            {
                return Result.Failure<QuestionResponse>(PollsErrors.PollNotFound);
            }
            var QuestionIsExist = await _context.Questions.AnyAsync(x=>x.Content ==request.Content &&x.pollId ==pollId, cancellationToken: cancellationToken);

            if(QuestionIsExist)
            {
                return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

            }
            var question = request.Adapt<Question>();
            question.pollId = pollId;
            request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));
            await _context.AddAsync(question,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(question.Adapt<QuestionResponse>());

        }

        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {

         
            var question = await _context.Questions.Where(x => x.pollId == pollId &&x.Id==id)
                .Include(x => x.Answers)
                 .ProjectToType<QuestionResponse>()   
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);
            if(question is  null)
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
            return Result.Success(question);
        }

        public async Task<Result> ToggleStatuesAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x=>x.Id ==id &&x.pollId==pollId ,cancellationToken);
           
            if(question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound); 
            question.IsActive = !question.IsActive;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
