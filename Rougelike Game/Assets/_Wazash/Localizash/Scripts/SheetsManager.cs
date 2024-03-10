using UnityEngine;

namespace Wazash.Localizash
{

    /// <summary>
    /// Manages the localization data and downloads it from the Google Sheets document
    /// </summary>
    public class SheetsManager : MonoBehaviour
    {
        [SerializeField] private string googleSheetUrl;
        [SerializeField] private LocalizationData localizationData;

        private readonly SheetsDownloader downloader = new();
        private readonly DataParser parser = new();

        public void LoadLocalizationData() => StartCoroutine(downloader.DownloadSheetFromURL(googleSheetUrl, (jsonData) => parser.ParseData(jsonData, localizationData)));

    }
}
