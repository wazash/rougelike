using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public abstract class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Transform parentToReturnTo = null;

        private Vector2 offset;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        public Transform ParentToReturnTo { get => parentToReturnTo; set => parentToReturnTo = value; }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CalculateDragOffset(eventData);

            parentToReturnTo = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);

            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            CalculateDragPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(parentToReturnTo);

            canvasGroup.blocksRaycasts = true;
        }

        private void CalculateDragOffset(PointerEventData eventData)
        {
            offset = (Vector2)transform.position - eventData.position;
        }

        private void CalculateDragPosition(PointerEventData eventData)
        {
            Vector3 newPosition = eventData.position + offset;
            newPosition.x = Mathf.Clamp(newPosition.x, rectTransform.rect.width / 2, Screen.width - rectTransform.rect.width / 2);
            newPosition.y = Mathf.Clamp(newPosition.y, rectTransform.rect.height / 2, Screen.height - rectTransform.rect.height / 2);
            transform.position = newPosition;
        }
    }
}
