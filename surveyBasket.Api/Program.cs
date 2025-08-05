



var builder = WebApplication.CreateBuilder(args);
// Add services to the container.




builder.Services.AddDependences(builder.Configuration);

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())

{
    //app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSwagger(options => options.OpenApiVersion =
     Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);

}

app.UseHttpsRedirection();
app.UseCors(); // middelware
app.UseAuthorization();

app.MapControllers();

app.Run();
