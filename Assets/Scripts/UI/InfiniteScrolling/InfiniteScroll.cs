using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.InfiniteScrolling
{
    public class InfiniteScroll : MonoBehaviour, IScrollHandler
    {
        [FormerlySerializedAs("scrollContent")] [SerializeField]
        private ScrollContentController scrollContentController;
        
        [SerializeField]
        private float outOfBoundsThreshold;
        
        private ScrollRect _scrollRect;

        private bool _positiveDrag;
        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _scrollRect.vertical = scrollContentController.Vertical;
            _scrollRect.horizontal = scrollContentController.Horizontal;
            _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        }
        
        public void OnScroll(PointerEventData eventData)
        {
            _positiveDrag = eventData.scrollDelta.y < 0;
        }
        
        public void OnViewScroll()
        {
            if (scrollContentController.Vertical)
            {
                HandleVerticalScroll();
            }
            else
            {
                HandleHorizontalScroll();
            }
        }

        private void HandleVerticalScroll()
        {
            var currItemIndex = _positiveDrag ? _scrollRect.content.childCount - 1 : 0;
            var currItem = _scrollRect.content.GetChild(currItemIndex);

            if (!ReachedThreshold(currItem))
            {
                return;
            }

            var endItemIndex = _positiveDrag ? 0 : _scrollRect.content.childCount - 1;
            var endItemRect = _scrollRect.content.GetChild(endItemIndex).GetComponent<RectTransform>();
            Vector2 newPos = endItemRect.localPosition;
            if (_positiveDrag)
            {
                newPos.y = endItemRect.localPosition.y - scrollContentController.ChildHeight - scrollContentController.ItemSpacing;
            }
            else
            {
                newPos.y = endItemRect.localPosition.y + scrollContentController.ChildHeight + scrollContentController.ItemSpacing;
            }

            currItem.localPosition = newPos;
            currItem.SetSiblingIndex(endItemIndex);
        }

        private void HandleHorizontalScroll()
        {
            int currItemIndex = _positiveDrag ? _scrollRect.content.childCount - 1 : 0;
            var currItem = _scrollRect.content.GetChild(currItemIndex);
            if (!ReachedThreshold(currItem))
            {
                return;
            }

            var endItemIndex = _positiveDrag ? 0 : _scrollRect.content.childCount - 1;
            var endItemRect = _scrollRect.content.GetChild(endItemIndex).GetComponent<RectTransform>();
            Vector2 newPos = endItemRect.localPosition;
            if (_positiveDrag)
            {
                newPos.x = endItemRect.localPosition.x - scrollContentController.ChildHeight - scrollContentController.ItemSpacing;
            }
            else
            {
                newPos.x = endItemRect.localPosition.x + scrollContentController.ChildHeight + scrollContentController.ItemSpacing;
            }

            currItem.localPosition = newPos;
            currItem.SetSiblingIndex(endItemIndex);
        }

        private bool ReachedThreshold(Transform item)
        {
            if (scrollContentController.Vertical)
            {
                var position = transform.position;
                var posYThreshold = position.y + scrollContentController.Height * 0.5f + outOfBoundsThreshold;
                var negYThreshold = position.y - scrollContentController.Height * 0.5f - outOfBoundsThreshold;
                return _positiveDrag ? item.position.y - scrollContentController.ChildWidth * 0.5f > posYThreshold :
                    item.position.y + scrollContentController.ChildWidth * 0.5f < negYThreshold;
            }
            else
            {
                var position = transform.position;
                var posXThreshold = position.x + scrollContentController.Width * 0.5f + outOfBoundsThreshold;
                var negXThreshold = position.x - scrollContentController.Width * 0.5f - outOfBoundsThreshold;
                return _positiveDrag ? item.position.x - scrollContentController.ChildWidth * 0.5f > posXThreshold :
                    item.position.x + scrollContentController.ChildWidth * 0.5f < negXThreshold;
            }
        }
    }
}