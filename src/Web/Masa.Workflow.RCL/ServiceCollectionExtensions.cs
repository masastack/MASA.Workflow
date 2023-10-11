using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Masa.Workflow.RCL;

public static class ServiceCollectionExtensions
{
    private static readonly JsonSerializerOptions s_defaultJsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static void AddMasaWorkflowUI(this IServiceCollection services)
    {
        services.AddScoped<DrawflowService>();
        services.AddMasaBlazor(options => { options.Defaults = GlobalConfig.ComponentDefaults; });
    }

    public static void AddMasaWorkflowActivities(this IServiceCollection services, Assembly assembly)
    {
        var activities = FindActivitiesInAssembly(assembly);
        services.AddOptions<WorkflowActivitiesRegistered>().Configure(options => { options.AddRange(activities); });

        services.AddGrpcClient<WorkflowAgent.WorkflowAgentClient>(o =>
        {
            o.Address = new Uri("https://localhost:6536");
        });
    }

    private static WorkflowActivitiesRegistered FindActivitiesInAssembly(Assembly assembly)
    {
        var assetFolderName = ".workflow";

        WorkflowActivitiesRegistered activities = new();

        var resourceNames = assembly.GetManifestResourceNames().Where(name => name.Contains(assetFolderName)).ToList();
        if (resourceNames.Count == 0)
        {
            return activities;
        }

        var resourceNamesGroups = resourceNames.GroupBy(u => u.Split(assetFolderName)[0]);
        foreach (var group in resourceNamesGroups)
        {
            WorkflowActivityRegistered workflowActivity = new();

            foreach (var resourceName in group)
            {
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null) continue;

                using var reader = new StreamReader(stream, Encoding.UTF8);
                var str = reader.ReadToEnd();

                if (str is null)
                {
                    continue;
                }

                if (resourceName.EndsWith(".config.json"))
                {
                    workflowActivity.Config = JsonSerializer.Deserialize<ActivityNodeConfig>(str, s_defaultJsonSerializerOptions);
                }
                else if (resourceName.Contains("locales"))
                {
                    var localeMd = resourceName.Split("locales.")[1];
                    var locale = localeMd.Split(".md")[0];
                    workflowActivity.Locales.Add(locale, str);
                }
            }

            if (workflowActivity.Config is null)
            {
                continue;
            }

            activities.Add(workflowActivity);
        }

        return activities;
    }
}
