using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Wazash.Localizash
{

    /// <summary>
    /// Downloads data from a Google Sheets document
    /// </summary>
    public class SheetsDownloader
    {
        public IEnumerator DownloadSheetFromURL(string url, Action<string> onComplete)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("URL is empty");
                yield break;
            }

            using UnityWebRequest webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Downloaded sheet data");
                    onComplete?.Invoke(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
