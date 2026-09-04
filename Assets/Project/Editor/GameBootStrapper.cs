#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Scripts.Core.Constant;

namespace Project.Editor
{
    public class GameBootStrapper
    {
        // Global シーンを経由しない開発時(各シーンを単体プレイした場合など)のフォールバック。
        // Entry シーン経由なら EntrySceneLifetimeScope 側の LoadInitialScenesUseCase が処理するのでスキップする
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void EnsureGlobalSceneLoaded()
        {
            var globalSceneName = SceneType.Global.ToString();
            var entrySceneName = SceneType.Entry.ToString();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == globalSceneName) return;
                if (scene.name == entrySceneName) return;
            }

            Debug.Log("[GameBootStrapper] Loading GlobalScene...");
            SceneManager.LoadScene(globalSceneName, LoadSceneMode.Additive);
        }
    }
}
#endif
