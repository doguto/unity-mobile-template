using Cysharp.Threading.Tasks;
using LitMotion;
using Project.Scripts.Application.ViewModel;

namespace Project.Scripts.Application.Service
{
    public class BlackCurtainService
    {
        public const float DefaultFadeDuration = 0.3f;

        readonly BlackCurtainViewModel viewModel;
        MotionHandle fadeHandle;

        public BlackCurtainService(BlackCurtainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public UniTask Close(float duration = DefaultFadeDuration) => FadeTo(1f, duration);

        public UniTask Open(float duration = DefaultFadeDuration) => FadeTo(0f, duration);

        async UniTask FadeTo(float targetAlpha, float duration)
        {
            // 途中で別のフェードが来た場合は現在の不透明度から繋ぎ直す
            if (fadeHandle.IsActive()) fadeHandle.Cancel();

            fadeHandle = LMotion.Create(viewModel.Alpha.Value, targetAlpha, duration)
                .WithEase(Ease.Linear)
                .Bind(viewModel, (alpha, vm) => vm.Alpha.Value = alpha);

            await fadeHandle.ToUniTask();
        }
    }
}
