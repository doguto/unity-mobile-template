using VContainer;
using VContainer.Unity;
using Project.Scenes.Sample.Scripts.Application.UseCase;
using Project.Scenes.Sample.Scripts.Application.ViewModel;
using Project.Scenes.Sample.Scripts.Repository.EntityRepository;
using Project.Scenes.Sample.Scripts.Repository.QueryRepository;
using Project.Scenes.Sample.Scripts.View;

namespace Project.Scenes.Sample.Scripts.Scope
{
    public class SampleSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Repository
            builder.Register<SampleEntityRepository>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<SampleCountQueryRepository>(Lifetime.Scoped).AsImplementedInterfaces();

            // ViewModel
            builder.Register<SampleViewModel>(Lifetime.Scoped);

            // UseCase
            builder.RegisterEntryPoint<InitializeSampleViewUseCase>(Lifetime.Scoped);
            builder.RegisterEntryPoint<IncrementSampleCountUseCase>(Lifetime.Scoped);

            // View
            builder.RegisterComponentInHierarchy<SampleView>();
        }
    }
}
