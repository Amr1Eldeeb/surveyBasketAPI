namespace surveyBasket.Api.Abstractions
{
    public static  class ResultExtensions
    {
        public static ObjectResult Toproplem(this Result result,int statusCode ,string title)
        {

            if(result.IsSuccess)
            {
                throw new InvalidOperationException
                    ("CanNot Convert success result to a problem");
            }
            //var problem = Results.Problem(statusCode: statusCode);
            //var proplemdetaliss  = problem.GetType().GetProperty(ProblemDetails).GetValue(problem) as ProblemDetails; 





            var problemDetalis = new ProblemDetails
           
            {
                Status = statusCode,
                Title = title,
                Extensions = new Dictionary<string, object?>
                {
                    {
                        "errors" , new object[]{result.Error}
                    }
                }
            };
            return new ObjectResult(problemDetalis);

        }
    }
}
