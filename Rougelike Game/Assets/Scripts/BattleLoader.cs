using StateMachine.BattleStateMachine;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class BattleLoader : MonoBehaviour
{
    [SerializeField] private EnemiesPack enemiesPack;

    private GameLoopStateMachine battleStateMachine;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        battleStateMachine = FindObjectOfType<GameLoopStateMachine>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(StartBattle);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(StartBattle);
    }

    private void StartBattle()
    {
        battleStateMachine.SetEnemiesPack(enemiesPack);
        battleStateMachine.SetState(typeof(BattleState));
    }
}
