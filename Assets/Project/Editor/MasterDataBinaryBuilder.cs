#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;
using Project.Scripts.Infrastructure;
using Project.Scripts.Infrastructure.MasterData;
using Project.Scripts.Repository;

namespace Project.Editor
{
    // マスタデータの元(ScriptableObject / CSV / スプレッドシート等)は問わない。
    // ここでは Assets 内の MasterDataAssetBase 派生アセットを集めて渡しているが、
    // 元データを ScriptableObject 以外に変えたい場合はこの収集処理だけ差し替えればよい
    public static class MasterDataBinaryBuilder
    {
        [MenuItem("Project/Master Data/Build Binary")]
        static void Build()
        {
            var assets = AssetDatabase.FindAssets($"t:{nameof(MasterDataAssetBase)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<MasterDataAssetBase>)
                .Where(asset => asset != null)
                .ToList();

            var binary = new BinaryGenerator().Generate(assets);
            new MasterDataCacheRepository().Save(binary);
            AssetDatabase.Refresh();
            Debug.Log($"[MasterDataBinaryBuilder] MasterData binary built from {assets.Count} asset(s).");
        }
    }
}
#endif
