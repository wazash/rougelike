using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "CustomUI/ThemeSO", fileName = "ThemeSO")]
    public class ThemeSO : ScriptableObject
    {
        [Header("Primary")]
        public Color Primary_BG;
        public Color Primary_Text;

        [Header("Secondary")]
        public Color Secondary_BG;
        public Color Secondary_Text;

        [Header("Tetriary")]
        public Color Tertiary_BG;
        public Color Tertiary_Text;

        [Header("Other")]
        public Color Disable;

        public Color GetBackgroundColor(Style style)
        {
            if (style == Style.Primary)
            {
                return Primary_BG;
            }
            else if( style == Style.Secondary)
            {
                return Secondary_BG;
            }
            else if ( style == Style.Tertiary)
            {
                return Tertiary_BG;
            }

            return Disable;
        }

        public Color GetTextColor(Style style)
        {
            if (style == Style.Primary)
            {
                return Primary_Text;
            }
            else if (style == Style.Secondary)
            {
                return Secondary_Text;
            }
            else if (style == Style.Tertiary)
            {
                return Tertiary_Text;
            }

            return Disable;
        }
    }
}
