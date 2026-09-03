using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Scripts.View
{
    public class SimpleButton : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Image image;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] float pressedScaleRatio = 0.95f;
        [SerializeField] float clickedAlpha = 0.85f;

        Transform myTransform;
        Vector3 defaultScale;

        readonly Subject<Unit> onClicked = new();
        public Observable<Unit> OnClicked => onClicked.AsObservable();

        readonly Subject<Unit> onFocused = new();
        public Observable<Unit> OnFocused => onFocused.AsObservable();

        readonly Subject<Unit> onUnfocused = new();
        public Observable<Unit> OnUnfocused => onUnfocused.AsObservable();

        protected override void Awake()
        {
            myTransform = transform;
            defaultScale = myTransform.localScale;
        }

        // タッチでは「押した感」を伝えるためスケールを縮める。PC マウスのホバー拡大は行わない
        public void OnPointerDown(PointerEventData eventData)
        {
            canvasGroup.alpha = clickedAlpha;
            myTransform.localScale = defaultScale * pressedScaleRatio;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            canvasGroup.alpha = 1;
            myTransform.localScale = defaultScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onFocused.OnNext(Unit.Default);
        }

        // 押下中に指が領域外へ流れた場合の見た目のリセット（スワイプによるタップキャンセル対策）
        public void OnPointerExit(PointerEventData eventData)
        {
            canvasGroup.alpha = 1;
            myTransform.localScale = defaultScale;
            onUnfocused.OnNext(Unit.Default);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClicked.OnNext(Unit.Default);
        }
    }
}
