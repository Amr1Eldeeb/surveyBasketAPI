﻿using surveyBasket.Api.Contracts.Questions;

namespace surveyBasket.Api.Services
{
    public interface IQuestionService
    {


    Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId,CancellationToken cancellationToken = default);    
    Task<Result<QuestionResponse>> AddAsync(int pollId,QuestionRequest request,CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>>GetAsync(int pollId,int id ,CancellationToken cancellationToken = default);

    Task<Result>ToggleStatuesAsync(int pollId,int id, CancellationToken cancellationToken = default);



    }
}
