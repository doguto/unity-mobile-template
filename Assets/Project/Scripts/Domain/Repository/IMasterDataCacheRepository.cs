namespace Project.Scripts.Domain.Repository
{
    public interface IMasterDataCacheRepository
    {
        byte[] Load();
        void Save(byte[] data);
    }
}
