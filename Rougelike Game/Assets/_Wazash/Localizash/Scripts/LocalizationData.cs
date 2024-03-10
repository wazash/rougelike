using System.Collections.Generic;
using UnityEngine;

namespace Wazash.Localizash
{

    [System.Serializable]
    public class LanguageEntry
    {
        public string languageCode;
        public string translation;
    }

    [System.Serializable]
    public class TranslationEntry
    {
        public string key;
        public List<LanguageEntry> translations = new();
    }

    [CreateAssetMenu(fileName = "LocalizationData", menuName = "Localization/LocalizationData", order = 1)]
    public class LocalizationData : ScriptableObject
    {
        public List<TranslationEntry> translations = new();

        public Dictionary<string, string> OriginalLanguageNames = new()
        {
            { "en", "English" },
            { "pl", "Polski" },
            { "es", "Español" },
            { "fr", "Français" },
            { "de", "Deutsch" }
            // Add more languages here
            // { "xx", "LanguageName" }
        };  

        public void AddEntry(string key, string languageCode, string translation)
        {
            TranslationEntry entry = translations.Find(x => x.key == key);
            if (entry == null)
            {
                entry = new TranslationEntry();
                entry.key = key;
                translations.Add(entry);
            }

            LanguageEntry languageEntry = entry.translations.Find(x => x.languageCode == languageCode);
            if (languageEntry == null)
            {
                languageEntry = new LanguageEntry();
                languageEntry.languageCode = languageCode;
                entry.translations.Add(languageEntry);
            }

            languageEntry.translation = translation;
        }

        public string GetText(string key, string languageCode)
        {
            TranslationEntry entry = translations.Find(x => x.key == key);
            if (entry != null)
            {
                LanguageEntry translation = entry.translations.Find(x => x.languageCode == languageCode);
                if (translation != null)
                {
                    return translation.translation;
                }
            }

            Debug.LogWarning($"Translation not found for key: {key} and language: {languageCode}");
            return key;
        }
    }
}
