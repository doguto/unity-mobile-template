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

            // 注意: この経路は LoadInitialScenesUseCase と違い、Global ロード後・単体シーン実行前に
            // マスタデータ準備完了(MasterDataReadyGate)を待ち合わせる隙間が無い。
            // 単体シーンは既にロードが始まっているため。実用上は数フレームで解決するため許容している
            Debug.Log("[GameBootStrapper] Loading GlobalScene...");
            SceneManager.LoadScene(globalSceneName, LoadSceneMode.Additive);
        }
    }
}
#endif
