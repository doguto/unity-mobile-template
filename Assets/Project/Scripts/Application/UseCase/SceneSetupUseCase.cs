using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Project.Scripts.Application.UseCase
{
    public abstract class SceneSetupUseCase : IAsyncStartable, ISceneSetup
    {
        readonly UniTaskCompletionSource completionSource = new();

        // StartAsync は PlayerLoop の Startup で発火するため Awake 直後はまだ走っていない。
        // 完了通知を挟むことで、待つ側は実行順序を気にせず済む
        public UniTask Completion => completionSource.Task;

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            // 失敗しても暗転したまま固まらないよう完了扱いにする。例外は VContainer のハンドラがログに出す
            try
            {
                await Execute(cancellationToken);
            }
            finally
            {
                completionSource.TrySetResult();
            }
        }

        protected abstract UniTask Execute(CancellationToken cancellationToken);
    }
}
