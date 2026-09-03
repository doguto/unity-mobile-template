using Project.Scripts.Domain.Repository;
using Project.Scripts.Infrastructure;

namespace Project.Scripts.Repository
{
    public class SetupDatabaseRepository : ISetupDatabaseRepository
    {
        readonly GameDatabase gameDatabase;

        public SetupDatabaseRepository(GameDatabase gameDatabase)
        {
            this.gameDatabase = gameDatabase;
        }

        public void SetupDatabase(byte[] dataBinary)
        {
            gameDatabase.SetupMaster(dataBinary);
        }
    }
}
