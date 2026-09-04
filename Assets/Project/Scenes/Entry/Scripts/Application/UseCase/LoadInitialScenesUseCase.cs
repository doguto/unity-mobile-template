using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using Project.Scripts.Core.Constant;

namespace Project.Scenes.Entry.Scripts.Application.UseCase
{
    // 実機起動時のエントリポイント。Global -> 初期シーンの順にAdditiveロードしてから
    // 自分自身(Entry)をアンロードする。Editor単体プレイ時はGameBootStrapperがフォールバックする
    public class LoadInitialScenesUseCase : IAsyncStartable
    {
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            var globalSceneName = SceneType.Global.ToString();
            var firstSceneName = SceneType.Sample.ToString();
            var entrySceneName = SceneType.Entry.ToString();

            await SceneManager.LoadSceneAsync(globalSceneName, LoadSceneMode.Additive).ToUniTask(cancellationToken: cancellationToken);
            await SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive).ToUniTask(cancellationToken: cancellationToken);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(firstSceneName));

            await SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(entrySceneName)).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}
