using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*builder.Services.AddSwaggerGen(opts =>
 opts.ResolveConflictingActions(apiDesc => apiDesc.First())
 );*/ // This is for resolving the route conflict and telling swagger to use the one which come first.However it is a bad practice
      // and we should delete the redundant route unless we actually know what we are doing.So for that we have "exclude from project" 
      // the ErrorController.cs file.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(cfg =>
    {
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
        cfg.AllowAnyMethod();
        cfg.AllowAnyOrigin();
    });
    options.AddPolicy(name: "AnyOrigin",
        cfg =>
        {
            cfg.AllowAnyOrigin();
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
        });
});
    


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Configuration.GetValue<bool>("UseSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}

if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");


app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapGet("/error",
    [EnableCors("AnyOrigin")] 
    [ResponseCache(NoStore = true)] () => 
    Results.Problem()); // this is a minimal api 
app.MapGet("/error/test", 
    [EnableCors("AnyOrigin")] 
    [ResponseCache(NoStore = true)] () => 
    { throw new Exception("test"); })
    .RequireCors("AnyOrigin");

/* 
this is for testing the Code On Demand constraint of RESTful APIs

app.MapGet("/cod/test",
 [EnableCors("AnyOrigin")]
[ResponseCache(NoStore = true)] () =>
 Results.Text("<script>" +
 "window.alert('Your client supports JavaScript!" +
 "\\r\\n\\r\\n" +
 $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
 "\\r\\n" +
 "Client time (UTC): ' + new Date().toISOString());" +
 "</script>" +
 "<noscript>Your client does not support JavaScript</noscript>",
 "text/html")); 
*/

app.MapControllers();

app.Run();
