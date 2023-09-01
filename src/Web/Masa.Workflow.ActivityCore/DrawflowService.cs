using System.Diagnostics.CodeAnalysis;

namespace Masa.Workflow.ActivityCore;

public class DrawflowService
{
    private MDrawflow? _drawflow;

    public void SetDrawflow(MDrawflow drawflow)
    {
        _drawflow = drawflow;
    }

    public async Task UpdateNodeDataFromIdAsync(int id, object data)
    {
        EnsureDrawflow();

        await _drawflow.UpdateNodeDataAsync(id, data).ConfigureAwait(false);
    }

    public async Task RemoveNodeAsync(int id)
    {
        EnsureDrawflow();

        await _drawflow.RemoveNodeAsync(id).ConfigureAwait(false);
    }

    public async Task AddNodeInputAsync(int id)
    {
        EnsureDrawflow();

        await _drawflow.AddInputAsync(id).ConfigureAwait(false);
    }

    public async Task AddNodeOutputAsync(int id)
    {
        EnsureDrawflow();

        await _drawflow.AddOutputAsync(id).ConfigureAwait(false);
    }

    public async Task RemoveNodeInputAsync(int id, string inputClass)
    {
        EnsureDrawflow();

        await _drawflow.RemoveInputAsync(id, inputClass).ConfigureAwait(false);
    }

    public async Task RemoveNodeOutputAsync(int id, string inputClass)
    {
        EnsureDrawflow();

        await _drawflow.RemoveOutputAsync(id, inputClass).ConfigureAwait(false);
    }

    public async Task<string?> ExportAsync()
    {
        EnsureDrawflow();

        return await _drawflow.ExportAsync().ConfigureAwait(false);
    }

    [MemberNotNull(nameof(_drawflow))]
    private void EnsureDrawflow()
    {
        if (_drawflow == null)
        {
            throw new NullReferenceException("Drawflow is not set.");
        }
    }
}
