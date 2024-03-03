using Units;
using UnityEngine;

namespace Cards
{
    public struct PlayingCardInfo
    {
        public Card Card;
        public Unit UnitTarget;
        public DropZone DropZone;
        public Transform TransformTarget;

        public PlayingCardInfo(Card card, Unit target, DropZone dropZone, Transform transformTarget = null)
        {
            Card = card;
            UnitTarget = target;
            DropZone = dropZone;
            TransformTarget = transformTarget;
        }
    }
}
