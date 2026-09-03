using R3;
using TMPro;
using UnityEngine;

namespace Project.Scripts.View
{
    public class TextButton : MonoBehaviour
    {
        [SerializeField] SimpleButton button;
        [SerializeField] TMP_Text label;

        public Observable<Unit> OnClicked => button.OnClicked;
        public Observable<Unit> OnFocused => button.OnFocused;

        public void SetLabel(string text) => label.text = text;
    }
}
