using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Project.Scripts.View
{
    // Screen.safeArea に自身の RectTransform をフィットさせる。
    // Update() での毎フレームポーリングではなく ILayoutSelfController として
    // Unity のレイアウトリビルド機構に乗せることで、リサイズ・回転時のみ再計算する。
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : UIBehaviour, ILayoutSelfController
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
            rectTransform.position = Screen.safeArea.position + Screen.safeArea.size * 0.5f;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.safeArea.size.x / canvas.scaleFactor);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.safeArea.size.y / canvas.scaleFactor);
        }

        void SetDirty()
        {
            if (!IsActive()) return;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}
