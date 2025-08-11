using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using surveyBasket.Api.Contracts.Questions;

namespace surveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        IQuestionService _questionService;
        public QuestionsController(IQuestionService questionService)
        {
            this._questionService = questionService;
        }

        [HttpGet("{id}")]

        public async Task<IActionResult>Get([FromRoute] int pollId, [FromRoute]int Id, CancellationToken cancellation)
        {
            var result = await _questionService.GetAsync(pollId, Id, cancellation);
            return result.IsSuccess ? Ok(result.value) : result.ToProblem(StatusCodes.Status404NotFound);
        }

        [HttpGet("")]
         public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellation)
        {
            var result = await _questionService.GetAllAsync(pollId, cancellation); 
            return result.IsSuccess ? Ok(result.value) : result.ToProblem(StatusCodes.Status404NotFound);
        }


        [HttpPost("")]
        public async Task<IActionResult> Add([FromRoute]int pollId, [FromBody]QuestionRequest request ,CancellationToken cancellation)
        {
            var result = await _questionService.AddAsync(pollId, request, cancellation);
            if(result.IsSuccess)
            {
                return CreatedAtAction(nameof(Get),new { pollId, result.value.Id},result.value);
            }
            return result.IsSuccess ? Ok(result) : 
                result.ToProblem(StatusCodes.Status400BadRequest);

        }
        [HttpPut("{id}/ToggleStatus")]
        public  async Task<IActionResult>ToggleStatus([FromRoute] int pollId, [FromRoute]int id ,CancellationToken cancellationToken)
        {
            var result = await _questionService.ToggleStatuesAsync(pollId, id, cancellationToken);
       
            return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status404NotFound);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Updated([FromRoute] int pollId, [FromRoute]int id, [FromBody] QuestionRequest request, CancellationToken cancellation)
        {
        var result = await _questionService.UpdateAsync(pollId , id, request, cancellation);  
           if(result.IsSuccess)
                return NoContent();

           return result.Error.Equals(QuestionErrors.DuplicatedQuestionContent) ?
                result.ToProblem(StatusCodes.Status409Conflict) :
                result.ToProblem(StatusCodes.Status404NotFound);

        }

    }
}
