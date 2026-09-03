using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using Project.Scripts.Application.UseCase;
using Project.Scripts.Core.Constant;
using Project.Scripts.Core.Scene;

namespace Project.Scripts.Application.Service
{
    public class SceneNavigatorService
    {
        readonly BlackCurtainService blackCurtainService;

        public SceneNavigatorService(BlackCurtainService blackCurtainService)
        {
            this.blackCurtainService = blackCurtainService;
        }

        public async UniTask NavigateTo(SceneType nextScene, SceneType fromScene, SceneParameter parameter = null)
        {
            var nextSceneName = nextScene.ToString();
            var fromSceneName = fromScene.ToString();
            Debug.Log($"[SceneNavigator] Start Scene Navigation: from {fromSceneName} to {nextSceneName}");

            await blackCurtainService.Close();

            if (parameter != null)
            {
                parameter.FromScene = fromScene;
                SceneParameterStore.Set(parameter);
            }

            await SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive).ToUniTask();

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextSceneName));
            await SceneManager.UnloadSceneAsync(fromSceneName).ToUniTask();

            // ロード直後は次シーンの Awake/Start が未実行のため、1 フレーム待ってから明転する
            await UniTask.NextFrame();

            await WaitForSceneSetup(nextScene);

            await blackCurtainService.Open();

            Debug.Log($"[SceneNavigator] Complete Scene Navigation: from {fromSceneName} to {nextSceneName}");
        }

        public async UniTask LoadAdditive(SceneType sceneType)
        {
            var sceneName = sceneType.ToString();
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();
        }

        // 次シーンのセットアップが終わるまで暗転を維持し、初期化途中の画面を見せないようにする。
        // ISceneSetup は SceneSetupUseCase 経由で自動登録されるため、LifetimeScope 側の記述は不要
        static async UniTask WaitForSceneSetup(SceneType sceneType)
        {
            var lifetimeScope = FindLifetimeScope(sceneType);
            if (lifetimeScope == null) return;

            // セットアップを持たないシーンでは解決に失敗するため、待たずに明転する
            if (!lifetimeScope.Container.TryResolve<ISceneSetup>(out var sceneSetup)) return;

            Debug.Log($"[SceneNavigator] Waiting Scene Setup: {sceneType}");
            await sceneSetup.Completion;
        }

        static LifetimeScope FindLifetimeScope(SceneType sceneType)
        {
            var scene = SceneManager.GetSceneByName(sceneType.ToString());
            if (!scene.IsValid()) return null;

            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                var lifetimeScope = rootGameObject.GetComponentInChildren<LifetimeScope>(true);
                if (lifetimeScope != null && lifetimeScope.Container != null) return lifetimeScope;
            }

            Debug.LogWarning($"[SceneNavigator] LifetimeScope Not Found: {sceneType}");
            return null;
        }
    }
}
