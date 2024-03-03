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

        public string TypeName { get => typeName; }
        public ElementalType[] Strengths { get => strengths; }
        public ElementalType[] Weaknesses { get => weaknesses;  }
        public ElementalType[] Immunesses { get => immunesses; }

        public float GetEffectivnessMultiplier(ElementalType otherType)
        {
            if (strengths.Contains(otherType))
                return 2.0f;
            else if (weaknesses.Contains(otherType))
                return 0.5f;
            else if (immunesses.Contains(otherType))
                return 0.0f;
            else
                return 1.0f;
        }

        private void OnValidate() => ChangeTypeNameBasedOnFileName();

        private void ChangeTypeNameBasedOnFileName()
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            string assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            typeName = assetName;
        }
    }
}
