using UnityEngine;
using Project.Scripts.Application.UseCase;
using Project.Scripts.Domain.Repository;

namespace Project.Scenes.Global.Scripts.Application.UseCase
{
    public class SetupDatabaseUseCase : InitializeUseCase
    {
        readonly IMasterDataCacheRepository masterDataCacheRepository;
        readonly ISetupDatabaseRepository setupDatabaseRepository;

        public SetupDatabaseUseCase(
            IMasterDataCacheRepository masterDataCacheRepository,
            ISetupDatabaseRepository setupDatabaseRepository
        )
        {
            this.masterDataCacheRepository = masterDataCacheRepository;
            this.setupDatabaseRepository = setupDatabaseRepository;
        }

        protected override void Execute()
        {
            var dataBinary = masterDataCacheRepository.Load();
            setupDatabaseRepository.SetupDatabase(dataBinary);
            Debug.Log("Database Setup.");
        }
    }
}
