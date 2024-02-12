using TMPro;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "CustomUI/TextSO", fileName = "TextSO")]
    public class TextSO : ScriptableObject
    {
        public ThemeSO Theme;

        public TMP_FontAsset Font;
        public float Size;
    }
}
