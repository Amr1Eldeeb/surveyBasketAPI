using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace surveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultServices _services;
        public ResultsController(IResultServices resultServices)
        {
           _services = resultServices;
        }
        [HttpGet("row-data")]
        public async Task<IActionResult> PollVote([FromRoute]int pollId ,CancellationToken cancellationToken )
        {
            var result  = await _services.GetPollVoteAsync(pollId,cancellationToken);
            return result.IsSuccess ? Ok(result) : result.ToProblem();
        }

    }
}
