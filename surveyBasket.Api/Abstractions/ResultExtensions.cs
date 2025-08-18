using Microsoft.AspNetCore.Components.Forms;

namespace surveyBasket.Api.Abstractions
{
    public static  class ResultExtensions
    {
        public static ObjectResult ToProblem(this Result result  )
        {

            if(result.IsSuccess)
            {
                throw new InvalidOperationException
                    ("Cannot Convert success result to a problem");
            }


            var problem = Results.Problem(statusCode: result.Error.StatusCode);
            var problemdetalis = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem)  as ProblemDetails;

            problemdetalis!.Extensions = new Dictionary<string, object?>
            {
                {
                     "errors" , new object[]
                     {
                         result.Error.Code,
                         result.Error.ErrorDescription
                     
                     }
                }
            };


            // var problemDetalis = new ProblemDetails
           
            //{
            //    Type = "",
            //    Status = statusCode,
            //    Title = title,
            //    Extensions = new Dictionary<string, object?>
            //    {
            //        {
            //            "errors" , new object[]{result.Error}
            //        }
            //    }
            //};
            return new ObjectResult(problemdetalis);

        }
    }
}
