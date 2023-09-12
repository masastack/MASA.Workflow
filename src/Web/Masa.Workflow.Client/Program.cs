var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddDaprWorkflow(options =>
{
    options.RegisterWorkflow<OrderProcessingWorkflow>();

    options.RegisterActivity<InventoryActivity>();
    options.RegisterActivity<InventoryActivity2>();
});

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMasaBlazor();
builder.Services.AddMasaWorkflowUI();

await builder.Build().RunAsync();
