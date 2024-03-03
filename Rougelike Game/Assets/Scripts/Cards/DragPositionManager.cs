using System.Collections.Generic;
using UnityEngine.UI;

namespace Cards
{
    public class DragPositionManager
    {
        private readonly Dictionary<Draggable, int> initialPosition = new();

        public void RegisterStartPosition(Draggable draggable)
        {
            if (draggable.transform.parent != null && draggable.transform.parent.GetComponent<HorizontalLayoutGroup>() != null)
            {
                initialPosition[draggable] = draggable.transform.GetSiblingIndex();
            }
        }

        public void ResetToStartPosition(Draggable draggable)
        {
            if (initialPosition.TryGetValue(draggable, out int startIndex))
            {
                draggable.transform.SetSiblingIndex(startIndex);
                initialPosition.Remove(draggable);
            }
        }
    }
}
