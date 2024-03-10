using Newtonsoft.Json;
using System.Linq;

namespace Wazash.Localizash
{
    #region Json valid blueprint
    // json blueprint for the Google Sheets JSON document
    // {
    //  "values": [
    //     [
    //         "Keys",
    //         "{key_1}",
    //         "{key_2}",
    //         "{key_3}"
    //     ],
    //     [
    //         "{lang_code}",
    //         "{translation_key_1}",
    //         "{translation_key_2}",
    //         "{translation_key_3}"
    //     ],
    //     [
    //         ...
    //     ]
    //  ] 
    // }
    #endregion

    /// <summary>
    /// Parses the data from the JSON document and populates the LocalizationData. 
    /// Need newtonsoft.json package to work (com.unity.nuget.newtonsoft-json | git url)
    /// </summary>
    public class DataParser
    {
        public void ParseData(string jsonData, LocalizationData localizationData)
        {
            if (string.IsNullOrEmpty(jsonData))
            {
                UnityEngine.Debug.Log("JSON data is empty");
                return;
            }
            if (localizationData == null)
            {
                UnityEngine.Debug.Log("LocalizationData is not assigned");
                return;
            }

            localizationData.translations.Clear();

            SheetData data = JsonConvert.DeserializeObject<SheetData>(jsonData);
            var keys = data.values[0].Skip(1).ToList();

            for (int columnIndex = 1; columnIndex < data.values.Count; columnIndex++)
            {
                var column = data.values[columnIndex];
                string languageCode = column[0];

                for (int rowIndex = 1; rowIndex < column.Count; rowIndex++)
                {
                    string key = keys[rowIndex - 1];
                    string translation = column[rowIndex];
                    localizationData.AddEntry(key, languageCode, translation);
                }
            }

            UnityEngine.Debug.Log("Data parsed successfully");
        }
    }
}
