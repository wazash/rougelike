using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public abstract class CustomUIComponent : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        public abstract void Setup();
        public abstract void Configure();

        [Button("Configure Now")]
        private void Init()
        {
            Setup();
            Configure();
        }

        private void OnValidate()
        {
            Init();
        }
    }

}
