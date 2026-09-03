using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Scripts.Application.Orchestrator
{
    public interface IOrchestrator
    {
        UniTask RunAsync(CancellationToken ct);
    }
}
