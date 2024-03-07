using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TMPLorem
{
    [CustomEditor(typeof(TMPWithLorem))]
    public class TMPWithLoremEditor : Editor
    {
        [MenuItem("GameObject/UI/TextMeshPro - Lorem Ipsum", false, 7)]
        private static void CreateTMPWithLorem(MenuCommand menuCommand)
        {
            GameObject go = new("TMP Lorem Ipsum");
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new(200, 200);

            go.AddComponent<TMPWithLorem>();

            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasGameObject = new("Canvas");
                canvas = canvasGameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGameObject.AddComponent<CanvasScaler>();
                canvasGameObject.AddComponent<GraphicRaycaster>();
            }

            go.transform.SetParent(canvas.transform, false);
            Selection.activeObject = go;

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TMPWithLorem script = (TMPWithLorem)target;

            #region MinMax words slider and fields Horizontal
            EditorGUILayout.BeginHorizontal();

            float min = script.WordsAmount.MinValue;
            float max = script.WordsAmount.MaxValue;

            EditorGUILayout.PrefixLabel("Words per Sentence");

            GUILayout.FlexibleSpace();

            EditorGUILayout.MinMaxSlider(ref min, ref max, 5, 50/*, GUILayout.MinWidth(100)*/);

            script.WordsAmount.MinValue = Mathf.RoundToInt(min);
            script.WordsAmount.MaxValue = Mathf.RoundToInt(max);

            script.WordsAmount.MinValue = EditorGUILayout.IntField(script.WordsAmount.MinValue, GUILayout.Width(50));
            script.WordsAmount.MaxValue = EditorGUILayout.IntField(script.WordsAmount.MaxValue, GUILayout.Width(50));

            EditorGUILayout.EndHorizontal(); 
            #endregion

            // Clamping words values
            script.WordsAmount.MinValue = Mathf.Clamp(script.WordsAmount.MinValue, script.Min_words, script.WordsAmount.MaxValue);
            script.WordsAmount.MaxValue = Mathf.Clamp(script.WordsAmount.MaxValue, script.WordsAmount.MinValue, script.Max_words);

            script.WordsAmount.MinValue = Mathf.Min(script.WordsAmount.MinValue, script.WordsAmount.MaxValue);
            script.WordsAmount.MaxValue = Mathf.Max(script.WordsAmount.MinValue, script.WordsAmount.MaxValue);

            #region Buttons Horizontal
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate Lorem", GUILayout.MinWidth(100)))
            {
                Undo.RecordObject(script, "Generate Lorem Ipsum");
                script.GenerateLorem();
            }

            if (GUILayout.Button("Clear", GUILayout.MinWidth(100)))
            {
                Undo.RecordObject(script, "Clear");
                script.Clear();
            }

            EditorGUILayout.EndHorizontal(); 
            #endregion

            EditorUtility.SetDirty(script);
        }
    }
}
