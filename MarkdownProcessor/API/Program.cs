using BusinessLogic;
using BusinessLogic.Services;
using Persistence;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<JwtService>();

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddAuth(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/MainPage/index.html");
});

app.Run();
