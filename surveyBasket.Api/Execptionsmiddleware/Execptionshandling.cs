//namespace surveyBasket.Api.Execptionsmiddleware
//{
//    public class Execptionshandling(RequestDelegate next, ILogger<Execptionshandling> execptions)

//    {
//        private readonly RequestDelegate _next  = next;

//        private readonly ILogger<Execptionshandling> logger  = execptions;

//        public async Task InvokeAsync(HttpContext httpcontext)
//        {
//            try
//            {
//                await _next(httpcontext);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "SomeThing went wrong {Massage}", ex.Message);
//                var problemdetalis = new ProblemDetails
//                {
//                    Status = StatusCodes.Status500InternalServerError,
//                    Title = "Internal Server Error",
//                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
                   
//                };

//                httpcontext.Response.StatusCode = StatusCodes.Status500InternalServerError;
//                await httpcontext.Response.WriteAsJsonAsync(problemdetalis);

//            }



//        }


//        //كنا بسنخدم الباترن دا قبل دوت نت 8 ونعمله تسجيل ف البيروجرم







//    }
//}
