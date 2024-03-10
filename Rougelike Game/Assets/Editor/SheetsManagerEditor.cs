using UnityEditor;
using UnityEngine;
using Wazash.Localizash;

[CustomEditor(typeof(SheetsManager))]
public class SheetsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SheetsManager sheetsManager = (SheetsManager)target;
        if (GUILayout.Button("Fetch Data"))
        {
            sheetsManager.LoadLocalizationData();
        }
    }
}
