using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.View
{
    public class IconButton : MonoBehaviour
    {
        [SerializeField] SimpleButton button;
        [SerializeField] Image icon;

        public Observable<Unit> OnClicked => button.OnClicked;
        public Observable<Unit> OnFocused => button.OnFocused;

        public void SetIcon(Sprite sprite) => icon.sprite = sprite;
    }
}
