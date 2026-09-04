using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using Project.Scripts.Application.Service;
using Project.Scripts.Core.Constant;

namespace Project.Scenes.Entry.Scripts.Application.UseCase
{
    public class LoadInitialScenesUseCase : IAsyncStartable
    {
        readonly MasterDataReadyGate masterDataReadyGate;

        public LoadInitialScenesUseCase(MasterDataReadyGate masterDataReadyGate)
        {
            this.masterDataReadyGate = masterDataReadyGate;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            var globalSceneName = SceneType.Global.ToString();
            var firstSceneName = SceneType.Sample.ToString();
            var entrySceneName = SceneType.Entry.ToString();

            await SceneManager.LoadSceneAsync(globalSceneName, LoadSceneMode.Additive).ToUniTask(cancellationToken: cancellationToken);
            await masterDataReadyGate.WaitAsync().AttachExternalCancellation(cancellationToken);
            await SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive).ToUniTask(cancellationToken: cancellationToken);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(firstSceneName));

            // Entry シーン自身をアンロードする操作。ここで scope 由来の cancellationToken を渡すと、
            // アンロードが EntrySceneLifetimeScope 自身を破棄してtoken自身をキャンセルしてしまい、
            // 完了を待っているこのawaitがOperationCanceledExceptionになる自己参照になるため渡さない
            await SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(entrySceneName)).ToUniTask();
        }
    }
}
