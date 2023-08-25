using System.Text.Json.Serialization;

namespace Masa.Workflow.ActivityCore;

public class ActivityMeta
{
    private int _input;
    private int _output;

    public ActivityMeta()
    {
    }

    protected ActivityMeta(string name, string icon, int input, int output)
    {
        Name = name;
        Icon = icon;
        Input = input;
        Output = output;

        InputLabels = Enumerable.Repeat<string?>(null, input).ToList();
        OutputLabels = Enumerable.Repeat<string?>(null, output).ToList();
    }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string Type { get; set; } = null!;

    public bool Disabled { get; set; }

    public string? Property { get; set; }

    public List<List<Guid>>? Wires { get; set; }

    public string Icon { get; set; } = null!;

    public bool IconRight { get; set; }

    public bool HideLabel { get; set; }

    public int Input
    {
        get => _input;
        set
        {
            _input = value;

            if (_input < MinInput)
            {
                _input = MinInput;
            }

            if (_input < InputLabels.Count)
            {
                InputLabels = InputLabels.Take(_input).ToList();
            }
            else
            {
                InputLabels.AddRange(Enumerable.Repeat<string?>(null, _input - InputLabels.Count));
            }
        }
    }

    public int Output
    {
        get => _output;
        set
        {
            _output = value;

            if (value < MinOutput)
            {
                _output = MinOutput;
            }

            if (_output < OutputLabels.Count)
            {
                OutputLabels = OutputLabels.Take(_output).ToList();
            }
            else if (_output > OutputLabels.Count)
            {
                OutputLabels.AddRange(Enumerable.Repeat<string?>(null, _output - OutputLabels.Count));
            }
        }
    }

    public int MinInput { get; set; }

    public int MinOutput { get; set; }

    public List<string?> InputLabels { get; set; } = new();

    public List<string?> OutputLabels { get; set; } = new();

    [JsonIgnore]
    public string DisplayName => Name ?? Type;

    public string Meta { get; set; }
}

public class ActivityMeta<TData> : ActivityMeta where TData : class, new()
{
    public ActivityMeta()
    {
    }

    protected ActivityMeta(string name, string icon, int input, int output) : base(name, icon, input, output)
    {
    }

    [JsonIgnore]
    [ValidateComplexType]
    public TData MetaData { get; set; } = new();
}
