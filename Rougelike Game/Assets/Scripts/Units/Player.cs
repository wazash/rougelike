using Sirenix.OdinInspector;
using UnityEngine;

namespace Units
{
    public class Player : Unit
    {
        [InlineEditor]
        [SerializeField] private PlayerData playerData;

        public PlayerData PlayerData => playerData;

        protected override void Start()
        {
            base.Start();

            healthComponent.OnDeath += OnDeath;
        }

        internal void SetPlayerData(PlayerData playerData)
        {
            Debug.Log("Setting player data");
            this.playerData = playerData;
        }

        private void OnDeath()
        {
            // Player death logic
        }
    }
}
