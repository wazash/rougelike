using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CustomView : CustomUIComponent
    {
        public ViewSO viewData;

        public GameObject ContainerTop;
        public GameObject ContainerCenter;
        public GameObject ContainerBottom;

        private Image imageTop;
        private Image imageCenter;
        private Image imageBottom;

        private VerticalLayoutGroup verticalLayoutGroup;

        public override void Setup()
        {
            verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            imageTop = ContainerTop.GetComponent<Image>();
            imageCenter = ContainerCenter.GetComponent<Image>();
            imageBottom = ContainerBottom.GetComponent<Image>();
        }

        public override void Configure()
        {
            verticalLayoutGroup.padding = viewData.Padding;
            verticalLayoutGroup.spacing = viewData.Spacing;

            imageTop.color = viewData.Theme.Primary_BG;
            imageCenter.color = viewData.Theme.Secondary_BG;
            imageBottom.color = viewData.Theme.Tertiary_BG;
        }
    }
}
