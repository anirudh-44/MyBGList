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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.UseAuthorization();

app.MapGet("/error", () => Results.Problem()); // this is a minimal api 
app.MapGet("/error/test", () => { throw new Exception("test"); });
app.MapControllers();

app.Run();
