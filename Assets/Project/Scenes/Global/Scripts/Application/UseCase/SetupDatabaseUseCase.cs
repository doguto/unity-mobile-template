using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Project.Scripts.Application.Service;
using Project.Scripts.Domain.Repository;

namespace Project.Scenes.Global.Scripts.Application.UseCase
{
    public class SetupDatabaseUseCase : IAsyncStartable
    {
        readonly IMasterDataCacheRepository masterDataCacheRepository;
        readonly ISetupDatabaseRepository setupDatabaseRepository;
        readonly MasterDataReadyGate masterDataReadyGate;

        public SetupDatabaseUseCase(
            IMasterDataCacheRepository masterDataCacheRepository,
            ISetupDatabaseRepository setupDatabaseRepository,
            MasterDataReadyGate masterDataReadyGate
        )
        {
            this.masterDataCacheRepository = masterDataCacheRepository;
            this.setupDatabaseRepository = setupDatabaseRepository;
            this.masterDataReadyGate = masterDataReadyGate;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            var dataBinary = await masterDataCacheRepository.LoadAsync();
            setupDatabaseRepository.SetupDatabase(dataBinary);
            masterDataReadyGate.Complete();
            Debug.Log("Database Setup.");
        }
    }
}
