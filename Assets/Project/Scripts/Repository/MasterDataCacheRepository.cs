using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Scripts.Core.Constant;
using Project.Scripts.Domain.Repository;

namespace Project.Scripts.Repository
{
    public class MasterDataCacheRepository : IMasterDataCacheRepository
    {
        public const string Address = "MasterDataCache";

        const string MasterDataCacheFileName = "MasterDataCache.bytes";

        // Save() はEditorの MasterDataBinaryBuilder からのみ呼ばれる、書き出し専用の保存先
        static string EditorSourcePath => Path.Combine(GamePath.DataStore, $"MasterData/{MasterDataCacheFileName}");

        public async UniTask<byte[]> LoadAsync()
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(Address);
            var textAsset = await handle.ToUniTask();
            var data = textAsset.bytes;
            Addressables.Release(handle);
            return data;
        }

        public void Save(byte[] data) => File.WriteAllBytes(EditorSourcePath, data);
    }
}
