using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Masa.Workflow.Activity;
using Masa.Workflow.Activity.Activities.Switch;
using Masa.Workflow.RCL.Models;
using Microsoft.Extensions.DependencyInjection;
using Index = Masa.Workflow.RCL.Pages.Index;

namespace Masa.Workflow.RCL;

public static partial class ServiceCollectionExtensions
{
    public static void AddMasaWorkflowUI(this IServiceCollection services)
    {
        services.AddScoped<DrawflowService>();
        services.AddMasaBlazor(options => { options.Defaults = GlobalConfig.ComponentDefaults; });

        var activities = NewMethod();
        // services.AddScoped(_ => activities);
        services.AddOptions<WorkflowActivitiesRegistered>().Configure(options => { options.AddRange(activities); });
    }

    private static JsonSerializerOptions DefaultOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    private static WorkflowActivitiesRegistered NewMethod()
    {
        const string assetFolderName = ".workflow";

        WorkflowActivitiesRegistered activities = new();


        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (assembly.GetTypes().Any(x
                    => x.BaseType is { IsGenericType: true } &&
                       typeof(MasaWorkflowActivity<>).IsAssignableFrom(x.BaseType.GetGenericTypeDefinition())))
            {
                var resourceNames = assembly.GetManifestResourceNames().Where(name => name.Contains(assetFolderName)).ToList();
                if (resourceNames.Count == 0)
                {
                    continue;
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
                            workflowActivity.Config = JsonSerializer.Deserialize<ActivityNodeConfig>(str, DefaultOptions);
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
            }
        }

        return activities;
    }
}

public class WorkflowActivitiesRegistered : List<WorkflowActivityRegistered>
{
}

public class WorkflowActivityRegistered
{
    public Dictionary<string, string> Locales { get; set; } = new();

    public ActivityNodeConfig? Config { get; set; }
}
