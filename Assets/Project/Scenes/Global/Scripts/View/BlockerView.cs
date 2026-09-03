using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Project.Scripts.Application.ViewModel;

namespace Project.Scenes.Global.Scripts.View
{
    public class BlockerView : MonoBehaviour
    {
        [SerializeField] Image blockerImage;

        BlockerViewModel blockerViewModel;
        DisposableBag disposableBag;

        [Inject]
        void Construct(BlockerViewModel blockerViewModel)
        {
            this.blockerViewModel = blockerViewModel;
        }

        void Start()
        {
            blockerViewModel.IsActive
                .Subscribe(isActive => blockerImage.raycastTarget = isActive)
                .AddTo(ref disposableBag);
        }

        void OnDestroy() => disposableBag.Dispose();
    }
}
