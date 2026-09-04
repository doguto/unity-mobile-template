#if UNITY_EDITOR

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Project.Scripts.Core.Constant;
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
        const string AddressableGroupName = "MasterData";

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

            RegisterAsAddressable();

            Debug.Log($"[MasterDataBinaryBuilder] MasterData binary built from {assets.Count} asset(s).");
        }

        // 実機ビルドに同梱されるよう、書き出した .bytes を固定Addressで登録する。
        // これによりEditor/実機どちらも同じAddressablesの経路でロードされ、挙動が分岐しない
        static void RegisterAsAddressable()
        {
            var assetPath = Path.Combine(GamePath.DataStore, "MasterData/MasterDataCache.bytes");
            var guid = AssetDatabase.AssetPathToGUID(assetPath);

            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            var group = settings.FindGroup(AddressableGroupName)
                ?? settings.CreateGroup(AddressableGroupName, false, false, true, null,
                    typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema),
                    typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.ContentUpdateGroupSchema));

            var entry = settings.CreateOrMoveEntry(guid, group, postEvent: false);
            entry.address = MasterDataCacheRepository.Address;

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
