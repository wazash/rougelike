using TMPro;
using UnityEngine;

namespace Cards
{
    public class CardView : MonoBehaviour, ICardView
    {
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI cardDescription;

        public void UpdateVisuals(CardData cardData)
        {
            if (cardData == null)
            {
                Debug.LogWarning("CardData is missing!");
                return;
            }

            cardName.text = cardData.Spell.SpellName;
            cardDescription.text = cardData.Spell.SpellDescription;
        }
    }

    public interface ICardView
    {
        public void UpdateVisuals(CardData cardData);
    }
}
