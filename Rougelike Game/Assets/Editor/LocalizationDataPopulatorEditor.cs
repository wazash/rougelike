using UnityEditor;
using UnityEngine;
using Wazash.Localizash;

[CustomEditor(typeof(LocalizationDataPopulator))]
public class LocalizationDataPopulatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LocalizationDataPopulator localizationDataPopulator = (LocalizationDataPopulator)target;
        if (GUILayout.Button("Populate Data"))
        {
            localizationDataPopulator.PopulateLocalizationData();
        }
    }
}
