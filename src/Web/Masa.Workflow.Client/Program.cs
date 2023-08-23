
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMasaBlazor();

var backendUrl = "https://localhost:6536";
builder.Services.AddGrpcClient<WorkflowServiceClient>(options =>
{
    options.Address = new Uri(backendUrl);

}).ConfigureChannel(options =>
{
    options.MaxRetryAttempts = 3;
    options.MaxReconnectBackoff = TimeSpan.FromSeconds(60);
});

await builder.Build().RunAsync();