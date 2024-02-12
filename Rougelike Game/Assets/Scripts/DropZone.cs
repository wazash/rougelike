using Card;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if( eventData.pointerDrag.TryGetComponent<Draggable>(out Draggable currentDragObject))
        {
            currentDragObject.ParentToReturnTo = this.transform;
        }
    }
}
