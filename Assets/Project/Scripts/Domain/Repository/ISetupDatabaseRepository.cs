namespace Project.Scripts.Domain.Repository
{
    public interface ISetupDatabaseRepository
    {
        void SetupDatabase(byte[] dataBinary);
    }
}
