var builder = WebApplication.CreateBuilder(args);

// dapr run --app-id masa-workflow --app-port 7129 --dapr-http-port 3501 dotnet run
//if (builder.Environment.IsDevelopment())
//{
//    builder.Services.AddDaprStarter(option =>
//    {
//        option.CreateNoWindow = false;
//        //option.EnableApiLogging = true;
//    });
//}

// Add services to the container.

builder.Services.AddMasaWorkflow();
builder.Services.AddEventBus();
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
    options.MaxReceiveMessageSize = 3 * 1024 * 1024;
    options.EnableDetailedErrors = !builder.Environment.IsProduction();
}).AddJsonTranscoding();

builder.Services
    .AddGrpcSwagger()
    .AddSwaggerGen(options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var filePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        options.IncludeXmlComments(filePath);
        options.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
    });

builder.Services.AddGrpcClient<WorkflowAgent.WorkflowAgentClient>(o =>
{
    o.Address = new Uri("https://localhost:6536");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.MapGrpcService<WorkflowRunnerService>();
app.MapGrpcService<WorkflowStarterService>();

app.Run();
