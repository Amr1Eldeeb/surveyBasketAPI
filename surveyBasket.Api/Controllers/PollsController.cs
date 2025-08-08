
namespace surveyBasket.Api.Controllers
{
    [Route("api/[controller]")] // template 
        [Authorize]
    [ApiController]
    public class PollsController(IPollServices pollservices) : ControllerBase
    {
        private readonly IPollServices _pollServices = pollservices;

        [HttpGet]
        [Route("")]
        // or [HttpGet("getAll)]
        //[DisableCors]// to not access to this endpoint 
        //[EnableCors("")] //to determine what is the policy allowed to access 
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _pollServices.GetAll(cancellationToken);
            
            return  result.IsSuccess ? Ok(result.value) :result.ToProblem(StatusCodes.Status400BadRequest);
        
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _pollServices.GetById(id, cancellationToken);

            return result.IsSuccess ? Ok(result.value) :


          Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.ErrorDescription);


             
        }


        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest request,
            CancellationToken cancellationToken)
        {

            var NewPoll = await _pollServices.Add(request, cancellationToken);
         
            
            return NewPoll.IsSuccess ?CreatedAtAction(nameof(GetById), new { id = NewPoll.value.Id }, NewPoll.value)   
                :NewPoll.ToProblem(StatusCodes.Status409Conflict);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _pollServices.Update(id, request, cancellationToken);
            return result.IsSuccess ? NoContent()
                : result.ToProblem(StatusCodes.Status409Conflict);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var z = id;
            var IsDeleted = await _pollServices.Delete(id, cancellationToken);
            if (IsDeleted.IsSuccess)
            {
                return NotFound();
            }
            return IsDeleted.ToProblem(StatusCodes.Status400BadRequest);

        }
        [HttpPut("{id}/togglePublished")]
        public async Task<IActionResult> TogglePublished([FromRoute] int id,
        CancellationToken cancellationToken)
        {
            var flag = await _pollServices.TogglePublishedStatues(id, cancellationToken);
            if (flag.IsFailure)
            {
                return flag.ToProblem(StatusCodes.Status400BadRequest);
            }
            return NoContent();
        }


    }
} 
