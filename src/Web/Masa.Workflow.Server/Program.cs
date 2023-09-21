using Masa.Workflow.Activity.Functions.Switch;
using Masa.Workflow.RCL.Hubs;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddMasaWorkflowUI();
builder.Services.AddMasaWorkflowActivities(typeof(Switch).Assembly);

builder.Services.AddServerSideBlazor(options => { options.RootComponents.RegisterCustomElementsUsedJSCustomElementAttribute(); });

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseResponseCompression();

app.MapBlazorHub();
app.MapHub<WorkflowClientHub>("/workflow-client-hub");
app.MapFallbackToPage("/_Host");

app.Run();
