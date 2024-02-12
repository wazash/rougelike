using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class CustomButton : CustomUIComponent
    {
        public ThemeSO Theme;
        public Style Style;
        public UnityEvent onClick;

        private Button button;
        private TextMeshProUGUI buttonText;

        public override void Setup()
        {
            button = GetComponentInChildren<Button>();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void Configure()
        {
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = Theme.GetBackgroundColor(Style);
            button.colors = colorBlock;

            buttonText.color = Theme.GetTextColor(Style);
        }

        public void OnClick()
        {
            onClick.Invoke();
        }
    }
}
