using R3;

namespace Project.Scripts.Application.ViewModel
{
    public class BlackCurtainViewModel
    {
        // 黒幕の不透明度。0: 完全に透明 / 1: 画面を黒で覆う
        public ReactiveProperty<float> Alpha { get; } = new(0f);
    }
}
