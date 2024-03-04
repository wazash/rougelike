using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    public abstract class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        protected Transform parentToReturnTo = null;
        protected Vector3 originalPosition;

        private Vector2 offset;
        protected RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        public Transform ParentToReturnTo { get => parentToReturnTo; }

        private void Start() => Setup();

        private void Setup()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            //SpawnPlaceHolder(gameObject);
            originalPosition = transform.position;
            CalculateDragOffset(eventData);

            parentToReturnTo = transform.parent;
            transform.SetParent(transform.parent.parent);

            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData) => CalculateDragPosition(eventData);

        public virtual void OnEndDrag(PointerEventData eventData) => canvasGroup.blocksRaycasts = true;

        private void CalculateDragOffset(PointerEventData eventData) => offset = (Vector2)transform.position - eventData.position;

        private void CalculateDragPosition(PointerEventData eventData)
        {
            Vector3 newPosition = eventData.position + offset;
            newPosition.x = Mathf.Clamp(newPosition.x, rectTransform.rect.width / 2, Screen.width - rectTransform.rect.width / 2);
            newPosition.y = Mathf.Clamp(newPosition.y, rectTransform.rect.height / 2, Screen.height - rectTransform.rect.height / 2);
            transform.position = newPosition;
        }
    }
}
