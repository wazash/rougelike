using Managers;
using StateMachine.BattleStateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class BattleNode : Node
    {
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(NodeRunner);
        }

        public override void NodeRunner()
        {
            Debug.Log("Battle node runned");
            GameManager.Instance.GameLoopStateMachine.SetState(typeof(InitializeBattleState));
        }
    }
}
