using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Project.Scripts.View
{
    // SafeAreaFitter の子要素に付けて、セーフエリアを無視しキャンバス全体に再フィットさせる。
    // 背景など「セーフエリア内の親配下に置きたいが表示自体は画面全体に広げたい」要素に使う。
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class CanvasFitter : UIBehaviour, ILayoutSelfController
    {
        RectTransform rectTransform;
        readonly DrivenRectTransformTracker tracker = new();

        public virtual void SetLayoutHorizontal() => Refresh();

        public virtual void SetLayoutVertical() => Refresh();

        protected override void OnRectTransformDimensionsChange() => SetDirty();

        protected override void OnTransformParentChanged() => SetDirty();

        protected override void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();

            // Tracker でロックする前にデフォルト状態を確定させておく
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;

            tracker.Add(this, rectTransform, DrivenTransformProperties.All);

            SetDirty();
        }

        protected override void OnDisable() => tracker.Clear();

        void Refresh()
        {
            var canvas = rectTransform.GetComponentInParent<Canvas>(includeInactive: true);
            var canvasRectTransform = canvas.GetComponent<RectTransform>();
            rectTransform.position = canvasRectTransform.position;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasRectTransform.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasRectTransform.rect.height);
        }

        void SetDirty()
        {
            if (!IsActive()) return;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}
