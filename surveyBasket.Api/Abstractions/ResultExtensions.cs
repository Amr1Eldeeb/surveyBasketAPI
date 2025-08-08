using Microsoft.AspNetCore.Components.Forms;

namespace surveyBasket.Api.Abstractions
{
    public static  class ResultExtensions
    {
        public static ObjectResult ToProblem(this Result result,int statusCode )
        {

            if(result.IsSuccess)
            {
                throw new InvalidOperationException
                    ("Cannot Convert success result to a problem");
            }


            var problem = Results.Problem(statusCode: statusCode);
            var problemdetalis = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem)  as ProblemDetails;

            problemdetalis!.Extensions = new Dictionary<string, object?>
            {
                {
                     "errors" , new object[]{result.Error}
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
