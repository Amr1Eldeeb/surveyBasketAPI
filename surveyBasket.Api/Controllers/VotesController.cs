

namespace surveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/Vote")]
    [ApiController]
    [Authorize]
    public class VotesController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;

        [HttpGet("")]
        public async Task<IActionResult> Start([FromRoute]int pollId , CancellationToken cancellationToken)
        {
            var userId  = User.GetUserId();
             
            var result = await _questionService.GetAvaliableAsync(pollId,userId!,cancellationToken);
            if (result.IsSuccess) return Ok(result.value);

            return result.Error.Equals(VoteErrors.DuplicatedVote)
                ?
                result.ToProblem(StatusCodes.Status409Conflict):
                result.ToProblem(StatusCodes.Status404NotFound);


        }




    }
}
