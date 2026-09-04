using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.View
{
    // EventSystem.pixelDragThreshold は物理ピクセル基準の値で、既定の10pxは低DPIのマウス操作向けのため、
    // 高DPI端末ではタップ時の指の僅かなブレでもドラッグ判定されクリックが成立しなくなる。DPI比でスケーリングして吸収する
    [RequireComponent(typeof(EventSystem))]
    public class EventSystemDpiScaler : MonoBehaviour
    {
        [SerializeField] float referenceDpi = 160f;
        [SerializeField] int baseDragThreshold = 10;

        void Awake()
        {
            var dpi = Screen.dpi > 0 ? Screen.dpi : referenceDpi;
            GetComponent<EventSystem>().pixelDragThreshold = Mathf.RoundToInt(baseDragThreshold * (dpi / referenceDpi));
        }
    }
}
