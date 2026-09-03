using R3;
using TMPro;
using UnityEngine;
using VContainer;
using Project.Scenes.Sample.Scripts.Application.ViewModel;
using Project.Scripts.View;

namespace Project.Scenes.Sample.Scripts.View
{
    public class SampleView : MonoBehaviour
    {
        [SerializeField] SimpleButton button;
        [SerializeField] TMP_Text countText;
        [SerializeField] TMP_Text nameText;

        SampleViewModel viewModel;
        DisposableBag disposableBag;

        [Inject]
        void Construct(SampleViewModel viewModel) => this.viewModel = viewModel;

        void Start()
        {
            viewModel.Count.Subscribe(count => countText.text = count.ToString()).AddTo(ref disposableBag);
            viewModel.Name.Subscribe(name => nameText.text = name).AddTo(ref disposableBag);
            button.OnClicked.Subscribe(_ => viewModel.OnClicked.OnNext(Unit.Default)).AddTo(ref disposableBag);
        }

        void OnDestroy() => disposableBag.Dispose();
    }
}
