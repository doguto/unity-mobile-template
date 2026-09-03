#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using Project.Scripts.Infrastructure;
using Project.Scripts.Repository;

namespace Project.Editor
{
    // マスタデータの元(ScriptableObject / CSV / スプレッドシート等)は問わない。
    // BinarGenerator.Generate() の中でテーブルデータを積む処理を実装したら、このメニューでバイナリ化する
    public static class MasterDataBinaryBuilder
    {
        [MenuItem("Project/Master Data/Build Binary")]
        static void Build()
        {
            var binary = new BinaryGenerator().Generate();
            new MasterDataCacheRepository().Save(binary);
            AssetDatabase.Refresh();
            Debug.Log("[MasterDataBinaryBuilder] MasterData binary built.");
        }
    }
}
#endif
