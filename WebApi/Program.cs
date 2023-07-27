using WebApi.Hubs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
//builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            ;
    });
});
WebApplication app = builder.Build();

app.UseCors();
app.MapHub<ChatHub>("chat");

app.MapGet("/", () => "Hello World!");

app.Run();
