using Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] private DropZoneType dropZoneType;

        public DropZoneType DropZoneType { get => dropZoneType; set => dropZoneType = value; }

        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag.TryGetComponent(out Card currentDragObject))
                return;

            if (dropZoneType == DropZoneType.Return)
                return;

            if (!TryGetComponent<Unit>(out var unit))
                return;

            currentDragObject.PlayCard(unit);
        }
    } 
}
