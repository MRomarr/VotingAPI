using VotingAPI;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voting API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}
app.UseCors(o=>o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
