using VContainer;
using VContainer.Unity;
using Project.Scenes.Global.Scripts.Application.UseCase;

namespace Project.Scenes.Global.Scripts.Scope
{
    public class GlobalSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SetupDatabaseUseCase>(Lifetime.Scoped);
        }
    }
}
