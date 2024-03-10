using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wazash.Localizash
{
    public interface ILocalizableUI
    {
        public string LocalizationKey { get; }

        public void UpdateLocalization(string text);
    }

    public class LocalizationManager : Singleton<LocalizationManager>
    {
        [SerializeField] private LocalizationData localizationData;
        [SerializeField] private string selectedLanguage = "en";

        private readonly List<ILocalizableUI> localizableUIs = new();

        public LocalizationData LocalizationData { get => localizationData; }
        public string SelectedLanguage { get => selectedLanguage; set => selectedLanguage = value; }

        public string GetText(string key)
        {
            return localizationData.GetText(key, selectedLanguage);
        }

        public void SetLanguage(string languageCode)
        {
            selectedLanguage = languageCode;
            UpdateLocalizableUIs();
        }

        public void RegisterLocalizableUI(ILocalizableUI localizableUI)
        {
            if (!localizableUIs.Contains(localizableUI))
            {
                localizableUIs.Add(localizableUI);
            }
        }
        public void UnregisterLocalizableUI(ILocalizableUI localizableUI)
        {
            if (localizableUIs.Contains(localizableUI))
            {
                localizableUIs.Remove(localizableUI);
            }
        }

        public void UpdateLocalizableUIs()
        {
            foreach (var localizableUI in localizableUIs)
            {
                string key = localizableUI.LocalizationKey;
                string text = GetText(key);
                localizableUI.UpdateLocalization(text);
            }
        }

        public IEnumerable<string> GetAvailableLanguages()
        {
            return localizationData.translations
                .SelectMany(entry => entry.translations)
                .Select(languageEntry => languageEntry.languageCode)
                .Distinct();
        }

        [ContextMenu("Register All And Update")]
        public void UpdateAllUIsInEditMode()
        {
            FindAllILocalizableUIsAndRegister();
            UpdateLocalizableUIs();
            FindAllILocalizableUIsAndUnregister();
        }

        private void FindAllILocalizableUIsAndRegister()
        {
            var localizableUIs = FindObjectsOfType<MonoBehaviour>().OfType<ILocalizableUI>();
            foreach (var localizableUI in localizableUIs)
            {
                RegisterLocalizableUI(localizableUI);
            }
        }

        private void FindAllILocalizableUIsAndUnregister()
        {
            var localizableUIs = FindObjectsOfType<MonoBehaviour>().OfType<ILocalizableUI>();
            foreach (var localizableUI in localizableUIs)
            {
                UnregisterLocalizableUI(localizableUI);
            }
        }
    }
}
