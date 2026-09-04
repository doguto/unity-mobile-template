using VContainer;
using VContainer.Unity;
using Project.Scenes.Entry.Scripts.Application.UseCase;

namespace Project.Scenes.Entry.Scripts.Scope
{
    public class EntrySceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LoadInitialScenesUseCase>(Lifetime.Scoped);
        }
    }
}
