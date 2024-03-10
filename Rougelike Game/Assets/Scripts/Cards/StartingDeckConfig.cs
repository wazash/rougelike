using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "NewDeckConfiguration", menuName = "Cards/Deck Configuration")]
    public class StartingDeckConfig : ScriptableObject
    {
        [SerializeField] private List<CardData> startingCardsData;

        public List<CardData> StartingCardsData { get => startingCardsData; }
    }
}
