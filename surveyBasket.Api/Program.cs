




using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddDependences(builder.Configuration);
//builder.Services.AddDistributedMemoryCache();





builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration) // دا عشان تروح بس علي فايل ال appSettings
);
//builder.Services.AddResponseCaching();
builder.Services.AddOutputCache();
var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 

{
    //app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSwagger(options => options.OpenApiVersion =
     Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
 
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors(); // middelware
app.UseAuthorization();
//builder.Services.AddOutputCache(options =>
//{
//    options.AddPolicy("polls", policy =>
//    {
//        policy.
//        Cache().
//        Expire(TimeSpan.FromSeconds(30)).Tag("AvaliableQuestions");
//    });
//});
//app.UseResponseCaching();

app.MapControllers();
app.UseExceptionHandler();  
//app.UseMiddleware<Execptionshandling>();

app.Run();
