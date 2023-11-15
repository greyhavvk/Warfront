using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InfiniteScrolling
{
    public class ScrollRectNoDrag : ScrollRect {
        public override void OnBeginDrag(PointerEventData eventData) { }
        public override void OnDrag(PointerEventData eventData) { }
        public override void OnEndDrag(PointerEventData eventData) { }
    }
}