using System.IO;
using UnityEngine;
using Project.Scripts.Core.Constant;
using Project.Scripts.Domain.Repository;

namespace Project.Scripts.Repository
{
    public class MasterDataCacheRepository : IMasterDataCacheRepository
    {
        const string MasterDataCacheFileName = "MasterDataCache.bytes";

        string CachePath
        {
            get
            {
#if UNITY_EDITOR
                return Path.Combine(GamePath.DataStore, $"MasterData/{MasterDataCacheFileName}");
#else
                return Path.Combine(Application.persistentDataPath, MasterDataCacheFileName);
#endif
            }
        }

        public byte[] Load() => File.ReadAllBytes(CachePath);

        public void Save(byte[] data) => File.WriteAllBytes(CachePath, data);
    }
}
