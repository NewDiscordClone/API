using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddControllers();
services.AddSwaggerGen(options =>
{
    string xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlName);
    options.IncludeXmlComments(xmlPath);
});
WebApplication app = builder.Build();

app.MapControllers();

app.MapSwagger();
app.UseSwaggerUI();

app.Run();
