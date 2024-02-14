using System.Linq;
using UnityEngine;

namespace Elementals
{
    [CreateAssetMenu(fileName = "New Elemental Type", menuName = "Elemental Types/Type")]
    public class ElementalType : ScriptableObject
    {
        [SerializeField] private string typeName;
        [SerializeField] private ElementalType[] strengths;
        [SerializeField] private ElementalType[] weaknesses;
        [SerializeField] private ElementalType[] immunesses;

        public string TypeName { get => typeName; set => typeName = value; }
        public ElementalType[] Strengths { get => strengths; set => strengths = value; }
        public ElementalType[] Weaknesses { get => weaknesses; set => weaknesses = value; }
        public ElementalType[] Immunesses { get => immunesses; set => immunesses = value; }

        public float GetEffectivnessMultiplier(ElementalType type)
        {
            if (strengths.Contains(type))
                return 2.0f;
            else if (weaknesses.Contains(type))
                return 0.5f;
            else if (immunesses.Contains(type))
                return 0.0f;
            else
                return 1.0f;
        }

        private void OnValidate()
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);

            string assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

            typeName = assetName;
        }
    }
}
