using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Project.Scripts.Application.ViewModel;

namespace Project.Scenes.Global.Scripts.View
{
    public class BlackCurtainView : MonoBehaviour
    {
        [SerializeField] Image curtainImage;

        BlackCurtainViewModel blackCurtainViewModel;
        DisposableBag disposableBag;

        [Inject]
        void Construct(BlackCurtainViewModel blackCurtainViewModel)
        {
            this.blackCurtainViewModel = blackCurtainViewModel;
        }

        void Start()
        {
            blackCurtainViewModel.Alpha
                .Subscribe(ApplyAlpha)
                .AddTo(ref disposableBag);
        }

        void ApplyAlpha(float alpha)
        {
            var color = curtainImage.color;
            color.a = alpha;
            curtainImage.color = color;

            // 完全に透明な間は下の UI へクリックを通す
            curtainImage.raycastTarget = alpha > 0f;
        }

        void OnDestroy() => disposableBag.Dispose();
    }
}
