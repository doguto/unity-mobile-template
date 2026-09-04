using Cysharp.Threading.Tasks;

namespace Project.Scripts.Application.Service
{
    // マスタデータのロード完了を、シーン読み込みの順序に依存せず待ち合わせるためのゲート。
    // SetupDatabaseUseCase が完了時に Complete() し、マスタデータに依存する処理側が WaitAsync() で待つ
    public class MasterDataReadyGate
    {
        readonly UniTaskCompletionSource completionSource = new();

        public UniTask WaitAsync() => completionSource.Task;

        public void Complete() => completionSource.TrySetResult();
    }
}
