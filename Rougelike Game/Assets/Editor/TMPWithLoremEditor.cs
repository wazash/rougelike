using UnityEditor;

namespace UnityEngine.UI
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

            var tmpWithLorem = go.AddComponent<TMPWithLorem>();

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

            if (GUILayout.Button("Generate Lorem Ipsum"))
            {
                Undo.RecordObject(script, "Generate Lorem Ipsum");
                script.GenerateLorem();
                EditorUtility.SetDirty(script);
            }
        }
    }
}
