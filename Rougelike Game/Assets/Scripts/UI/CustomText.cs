using TMPro;

namespace UI
{
    public class CustomText : CustomUIComponent
    {
        public TextSO TextData;
        public Style Style;

        private TextMeshProUGUI textMeshProUGUI;


        public override void Setup()
        {
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void Configure()
        {
            textMeshProUGUI.color = TextData.Theme.GetTextColor(Style);
            textMeshProUGUI.font = TextData.Font;
            textMeshProUGUI.fontSize = TextData.Size;
        }
    }
}
