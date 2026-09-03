using Project.Scenes.Sample.Scripts.Application.ViewModel;
using Project.Scenes.Sample.Scripts.Domain;
using Project.Scripts.Application.UseCase;

namespace Project.Scenes.Sample.Scripts.Application.UseCase
{
    // シーン表示直後、まだボタンを押していない状態(Count = 0)の Name を初期表示する
    public class InitializeSampleViewUseCase : InitializeUseCase
    {
        readonly SampleViewModel viewModel;
        readonly ISampleEntityRepository sampleEntityRepository;

        public InitializeSampleViewUseCase(SampleViewModel viewModel, ISampleEntityRepository sampleEntityRepository)
        {
            this.viewModel = viewModel;
            this.sampleEntityRepository = sampleEntityRepository;
        }

        protected override void Execute()
        {
            var entity = sampleEntityRepository.Get();
            viewModel.Count.Value = entity.Count;
            viewModel.Name.Value = entity.Name;
        }
    }
}
