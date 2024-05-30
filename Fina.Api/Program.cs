using Fina.Api;
using Fina.Api.Common.Api;
using Fina.Api.Endpoints;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

WebApplication? app = builder.Build();
if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseCors(ApiConfiguration.CorsPolicyName);
app.MapEndpoints();

app.Run();
