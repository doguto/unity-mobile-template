using Cysharp.Threading.Tasks;

namespace Project.Scripts.Domain.Repository
{
    public interface IMasterDataCacheRepository
    {
        UniTask<byte[]> LoadAsync();
        void Save(byte[] data);
    }
}
