using System.Linq;
using UnityEngine;


namespace Wazash.Localizash
{

    /// <summary>
    /// Manages the language dropdown allowing the user to change the language
    /// </summary>
    public class LanguageDropdownManager : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Dropdown dropdown;
        private LocalizationManager localizationManager;

        private void Awake()
        {
            localizationManager = LocalizationManager.Instance;
        }

        private void Start()
        {
            PopulateDropdown();

            SetDropdownValue(LocalizationManager.Instance.SelectedLanguage);

            dropdown.onValueChanged.AddListener(index => SetCurrentLanguageByName(index));
        }


        /// <summary>
        /// Sets the current language based on the dropdown language name
        /// </summary>
        /// <param name="index"></param>
        private void SetCurrentLanguageByName(int index)
        {
            string languageName = dropdown.options[index].text;
            string languageCode = localizationManager.LocalizationData.OriginalLanguageNames.FirstOrDefault(x => x.Value == languageName).Key;

            localizationManager.SetLanguage(languageCode);
        }

        /// <summary>
        /// Sets the dropdown value to the current language
        /// </summary>
        /// <param name="languageCode"></param>
        private void SetDropdownValue(string languageCode)
        {
            localizationManager.LocalizationData.OriginalLanguageNames.TryGetValue(languageCode, out string languageName);
            var index = dropdown.options.FindIndex(option => option.text == languageName);
            dropdown.value = index;
        }


        /// <summary>
        /// Populates the dropdown with the available languages
        /// </summary>
        private void PopulateDropdown()
        {
            if (localizationManager == null || dropdown == null)
            {
                Debug.LogError("LocalizationManager or dropdown not set in LanguageDropdownPopulator");
                return;
            }

            dropdown.options.Clear();

            var languageCodes = localizationManager.GetAvailableLanguages();

            foreach (var languageCode in languageCodes)
            {
                localizationManager.LocalizationData.OriginalLanguageNames.TryGetValue(languageCode, out string languageName);
                dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(languageName));
            }

            dropdown.RefreshShownValue();
        }
    }
}
