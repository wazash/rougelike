using UnityEngine;

namespace Cards
{
    [System.Serializable]
    public struct DeckPositions
    {
        public Transform MainDeckTransform;
        public Transform GameplayDeckTransform;
        public Transform DiscardDeckTransform;
        public Transform HandDeckTransform;

        public DeckPositions(Transform mainDeckTransform, Transform gameplayDeckTransform, Transform discardDeckTransform, Transform handDeckTransform)
        {
            MainDeckTransform = mainDeckTransform;
            GameplayDeckTransform = gameplayDeckTransform;
            DiscardDeckTransform = discardDeckTransform;
            HandDeckTransform = handDeckTransform;
        }
    }
}
