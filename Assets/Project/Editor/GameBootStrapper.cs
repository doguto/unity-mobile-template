#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Scripts.Core.Constant;

namespace Project.Editor
{
    public class GameBootStrapper
    {
        // Global シーンを経由しない開発時(各シーンを単体プレイした場合など)のフォールバック。
        // 何らかの起点シーン(Entry 相当)を作り、そこから Global を明示的にロードする構成になったら
        // 「起点シーンがロード済みならスキップ」の分岐をここに追加する
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void EnsureGlobalSceneLoaded()
        {
            var globalSceneName = SceneType.Global.ToString();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == globalSceneName) return;
            }

            Debug.Log("[GameBootStrapper] Loading GlobalScene...");
            SceneManager.LoadScene(globalSceneName, LoadSceneMode.Additive);
        }
    }
}
#endif
