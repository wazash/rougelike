using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public class UnitVisuals : MonoBehaviour
    {
        [SerializeField] private Image unitImage;

        private void Awake()
        {
            if(TryGetComponent(out Image image))
            {
                unitImage = image;
            }
            else
            {
                unitImage = GetComponentInChildren<Image>();
            }
        }

        public void SetSprite(Sprite sprite)
        {
            unitImage.sprite = sprite;
        }
    }
}
