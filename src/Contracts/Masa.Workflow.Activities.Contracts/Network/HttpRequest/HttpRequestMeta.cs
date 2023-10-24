namespace Masa.Workflow.Activities.Contracts.HttpRequest;

public class HttpRequestMeta
{
    public string Method { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string Headers { get; set; } = null!;

    public string QueryString { get; set; } = null!;

    public string Cookies { get; set; } = null!;

    public string Response { get; set; } = null!;
}
