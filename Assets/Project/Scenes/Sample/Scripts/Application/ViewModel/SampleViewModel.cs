using R3;

namespace Project.Scenes.Sample.Scripts.Application.ViewModel
{
    public class SampleViewModel
    {
        public Subject<Unit> OnClicked { get; } = new();      // 入力口
        public ReactiveProperty<int> Count { get; } = new(0); // 出力口
        public ReactiveProperty<string> Name { get; } = new("");
    }
}
