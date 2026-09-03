using Cysharp.Threading.Tasks;
using R3;
using Project.Scenes.Sample.Scripts.Application.ViewModel;
using Project.Scenes.Sample.Scripts.Domain;
using Project.Scripts.Application.UseCase;

namespace Project.Scenes.Sample.Scripts.Application.UseCase
{
    public class IncrementSampleCountUseCase : EventUseCase<Unit>
    {
        readonly SampleViewModel viewModel;
        readonly ISampleCountQueryRepository sampleCountQueryRepository;
        readonly ISampleEntityRepository sampleEntityRepository;

        public IncrementSampleCountUseCase(
            SampleViewModel viewModel,
            ISampleCountQueryRepository sampleCountQueryRepository,
            ISampleEntityRepository sampleEntityRepository
        )
        {
            this.viewModel = viewModel;
            this.sampleCountQueryRepository = sampleCountQueryRepository;
            this.sampleEntityRepository = sampleEntityRepository;
        }

        protected override Observable<Unit> CreateTrigger() => viewModel.OnClicked;

        protected override UniTask Execute(Unit value)
        {
            sampleCountQueryRepository.IncrementCount();

            var entity = sampleEntityRepository.Get();
            viewModel.Count.Value = entity.Count;
            viewModel.Name.Value = entity.Name;

            return UniTask.CompletedTask;
        }
    }
}
