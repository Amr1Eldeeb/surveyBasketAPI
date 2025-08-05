using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using surveyBasket.Api.Contracts.Polls;
using System.Reflection.Metadata.Ecma335;

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
            
            return  result.IsSuccess ? Ok(result.value) : BadRequest(PollsErrors.PollNotFound);
        
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _pollServices.GetById(id, cancellationToken);

            return result.IsSuccess ? Ok(result.value) :


          Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.ErrorDescription);
                
                ;


             
        }


        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] PollRequest request,
            CancellationToken cancellationToken)
        {

            var NewPoll = await _pollServices.Add(request, cancellationToken);
           var result =  NewPoll.value;
            var poll = _pollServices.GetAll();
            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _pollServices.Update(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);

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
            return NoContent();

        }
        [HttpPut("{id}/togglePublished")]
        public async Task<IActionResult> TogglePublished([FromRoute] int id,
        CancellationToken cancellationToken)
        {
            var flag = await _pollServices.TogglePublishedStatues(id, cancellationToken);
            if (flag.IsFailure)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
} 
