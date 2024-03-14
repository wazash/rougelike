using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wazash.Localizash;

[RequireComponent(typeof(LocalizedUI))]
public class ChangeContentToKey : MonoBehaviour
{
    private LocalizedUI localizedUI;
    private Button button;
    private TMP_Text text;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        
        if (TryGetComponent<Button>(out button))
        {
            text = button.GetComponentInChildren<TMP_Text>();
        }
        else
        {
            text = GetComponent<TMP_Text>();
        }

        localizedUI = GetComponent<LocalizedUI>();
    }

    private void ChangeContent(LocalizedUI localizedUI)
    {
        if(text == null)
        {
            Debug.LogWarning("No text component found.");
        }

        text.text = localizedUI.LocalizationKey;
    }

    private void ChangeObjectName(LocalizedUI localizedUI)
    {
        gameObject.name = button == null ? localizedUI.LocalizationKey + "_text" : localizedUI.LocalizationKey + "_button";
    }

    private void OnValidate()
    {
        GetReferences();

        ChangeContent(localizedUI);
        ChangeObjectName(localizedUI);
    }
}
