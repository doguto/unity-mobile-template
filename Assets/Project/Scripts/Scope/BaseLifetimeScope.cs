using VContainer;
using VContainer.Unity;
using Project.Scripts.Application.Service;
using Project.Scripts.Application.ViewModel;
using Project.Scripts.Infrastructure;
using Project.Scripts.Repository;

namespace Project.Scripts.Scope
{
    // Project 単位（全シーン共通）の DI 登録をここに置く。
    // Scene 単位の登録は Scenes/<SceneName>/Scripts/Scope/<SceneName>SceneLifetimeScope に置く
    public class BaseLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Service
            builder.Register<SceneNavigatorService>(Lifetime.Transient);
            builder.Register<BlockerService>(Lifetime.Singleton);
            builder.Register<BlackCurtainService>(Lifetime.Singleton);

            // ViewModel
            builder.Register<BlockerViewModel>(Lifetime.Singleton);
            builder.Register<BlackCurtainViewModel>(Lifetime.Singleton);

            // Infrastructure
            builder.Register<GameDatabase>(Lifetime.Singleton);
            builder.Register<RuntimeDatabase>(Lifetime.Singleton);

            // Repository
            builder.Register<MasterDataCacheRepository>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SetupDatabaseRepository>(Lifetime.Singleton).AsImplementedInterfaces();

            // TODO: EntityRepository / QueryRepository もここで登録する
        }
    }
}
