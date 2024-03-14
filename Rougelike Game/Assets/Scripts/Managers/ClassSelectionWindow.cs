using Sirenix.OdinInspector;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Wazash.Localizash;

namespace Managers
{

    public class ClassSelectionWindow : MonoBehaviour
    {
        [Title("Player Class")]
        [SerializeField] private PlayerData playerData;
        [Title("UI elements")]
        [SerializeField] private TMP_Text classNameText;
        [SerializeField] private LocalizedUI classNameKey;
        [SerializeField] private Image classImage;
        [SerializeField] private TMP_Text classDescriptionText;
        [SerializeField] private LocalizedUI classDescriptionKey;
        [SerializeField] private Button chooseButton;

        public Button ChooseButton => chooseButton;
        public PlayerData PlayerData => playerData;

        public void SetVisuals(PlayerData playerData)
        {
            this.playerData = playerData;
            classNameText.text = playerData.UnitName;
            classNameKey.LocalizationKey = playerData.UnitNameKey;
            classImage.sprite = playerData.UnitSprite;
            classDescriptionText.text = playerData.UnitDescription;
            classDescriptionKey.LocalizationKey = playerData.UnitDescriptionKey;
        }

        private void OnValidate()
        {
            if (playerData != null)
            {
                SetVisuals(playerData);
            }
        }
    }
}
