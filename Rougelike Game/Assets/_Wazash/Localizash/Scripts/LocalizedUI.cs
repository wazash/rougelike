using System.Collections.Generic;
using UnityEngine;

namespace Wazash.Localizash
{

    /// <summary>
    /// Component that updates its text with the localized text. Implements ILocalizableUI
    /// </summary>
    public class LocalizedUI : MonoBehaviour, ILocalizableUI
    {
        [SerializeField] private string key;
        private TMPro.TextMeshProUGUI tmpComponent;

        public string LocalizationKey => key;

        private void Awake()
        {
            // get the text component, if cannot find it, check if it is in the children
            if(!TryGetComponent<TMPro.TextMeshProUGUI>(out tmpComponent))
            {
                tmpComponent = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            }
        }

        private void Start()
        {
            LocalizationManager.Instance.RegisterLocalizableUI(this);
            UpdateLocalization(LocalizationManager.Instance.GetText(key));
        }

        /// <summary>
        /// Updates the text of the component with the given text
        /// </summary>
        /// <param name="text"></param>
        public void UpdateLocalization(string text)
        {
            if(tmpComponent == null)
            {
                tmpComponent = GetComponent<TMPro.TextMeshProUGUI>();
            }

            tmpComponent.text = text;
        }
    }
}
