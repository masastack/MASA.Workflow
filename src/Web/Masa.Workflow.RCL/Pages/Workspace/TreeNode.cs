namespace Masa.Workflow.RCL.Pages;

public class TreeNode
{
    public TreeNode(string label, string? icon = null, List<TreeNode>? children = null)
    {
        Key = label;
        Label = label;
        Icon = icon;
        Children = children;
    }

    public TreeNode(string key, string label, string? icon = null, string? color = null, TreeNodeType type = TreeNodeType.Node)
    {
        Key = key;
        Label = label;
        Icon = icon;
        Color = color;
        Type = type;
    }

    public string Key { get; set; }

    public string? Label { get; set; }

    public string? Icon { get; set; }

    public string? Color { get; set; }

    public TreeNodeType Type { get; set; }

    public List<TreeNode>? Children { get; set; }

    public Dictionary<string, string> Extra { get; set; } = new();
}

public enum TreeNodeType
{
    Text,

    Node,
}
