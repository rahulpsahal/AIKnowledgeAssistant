using AIKnowledgeAssistant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ollamaBaseUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
var ollamaTimeoutSeconds = builder.Configuration.GetValue<int>("Ollama:TimeoutSeconds", 120);

builder.Services.AddHttpClient<IAIService, AIService>(client =>
{
    client.BaseAddress = new Uri(ollamaBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(ollamaTimeoutSeconds);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
