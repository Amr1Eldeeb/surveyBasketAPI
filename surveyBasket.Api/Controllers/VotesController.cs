

using Microsoft.AspNetCore.OutputCaching;
using surveyBasket.Api.Contracts.Votes;

namespace surveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/Vote")]
    [ApiController]
    //[Authorize]
    public class VotesController(IQuestionService questionService,IVoteServices voteServices) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;

        public IVoteServices _voteServices  = voteServices;

        [HttpGet("")]
        //  [ResponseCache(Duration =60 )]  
        [OutputCache(PolicyName ="polls")]
        public async Task<IActionResult> Start([FromRoute]int pollId , CancellationToken cancellationToken)
        {
            //var userId  = User.GetUserId();
            var userId = "2a39603a-b8c8-41d1-80b4-00c1b941e521";   


            var result = await _questionService.GetAvaliableAsync(pollId,userId!,cancellationToken);
            if (result.IsSuccess) return Ok(result.value);

            return result.Error.Equals(VoteErrors.DuplicatedVote)
                ?
                result.ToProblem():
                result.ToProblem();


        }
        [HttpPost("")]
        public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] voteRequest request, CancellationToken cancellationToken)
        {
            var result = await _voteServices.AddVoteAsync(pollId, User.GetUserId()!, request, cancellationToken);
            if (result.IsSuccess)
                return Created();
            return result.ToProblem();
        }





    }
}
