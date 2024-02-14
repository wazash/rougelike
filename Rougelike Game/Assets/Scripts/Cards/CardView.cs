using TMPro;
using UnityEngine;

namespace Cards
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI cardDescription;

        public void UpdateVisuals(CardData cardData)
        {
            cardName.text = cardData.Spell.SpellName;
            cardDescription.text = cardData.Spell.SpellDescription;
        }
    }
}
