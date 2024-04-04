using Cards;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    public class BattleInitRoutines
    {
        private readonly GameLoopStateMachine machine;
        private readonly StartingDeckConfig deckConfiguration;
        private readonly Card cardPrefab;
        private readonly Transform gameplayDeckTransform;
        private readonly List<Transform> playerPositions;
        private readonly List<Transform> enemiesPositions;
        private readonly EnemiesPack enemiesPack;

        public BattleInitRoutines(GameLoopStateMachine machine, BattleInitData battleInitData, EnemiesPack enemiesPack)
        {
            this.machine = machine;
            deckConfiguration = battleInitData.deckConfiguration;
            cardPrefab = battleInitData.cardPrefab;
            gameplayDeckTransform = battleInitData.gameplayDeckTransform;
            playerPositions = battleInitData.playerPositions;
            enemiesPositions = battleInitData.enemiesPositions;
            this.enemiesPack = enemiesPack;
        }

        public IEnumerator PrepareDeckRoutine()
        {
            machine.DeckManager.InitializeMainDeck(deckConfiguration, cardPrefab, gameplayDeckTransform);
            machine.DeckManager.PrepareGameplayDeck();

            yield return new WaitForEndOfFrame();
        }

        public IEnumerator SetEnemiesRoutine()
        {
            yield return CoroutineRunner.Start(SpawnEnemiesForBattle(machine, enemiesPack));
            yield return CoroutineRunner.Start(MoveEnemiesToPositions(machine, enemiesPositions));
        }
        private IEnumerator SpawnEnemiesForBattle(GameLoopStateMachine machine, EnemiesPack enemiesPack)
        {
            for (int i = 0; i < enemiesPack.UnitList.Count; i++)
            {
                var enemy = enemiesPack.UnitList[i];
                CoroutineRunner.Start(enemy.SpawnEnemy(machine.GameManager.UnitsGroundManager.EnemySpawningPoint, machine.GameManager.UnitsGroundManager.EnemiesGround));
                yield return new WaitForEndOfFrame();
            }
        }
        private IEnumerator MoveEnemiesToPositions(GameLoopStateMachine machine, List<Transform> enemiesPositions)
        {
            for (int i = 0; i < machine.UnitsManager.Enemies.Count; i++)
            {
                var enemy = machine.UnitsManager.Enemies[i];
                enemy.SetMoveToPositionTween(enemiesPositions[i]);
                enemy.MoveToPositionTween.Play();
                yield return enemy.MoveToPositionTween.WaitForCompletion();
                enemy.IdleTween.Play();
            }
        }

        public IEnumerator SetPlayerRoutine()
        {
            yield return CoroutineRunner.Start(MovePlayerToPositions(machine, playerPositions));
        }
        private IEnumerator MovePlayerToPositions(GameLoopStateMachine machine, List<Transform> playerPositions)
        {
            var player = machine.UnitsManager.Player;
            player.SetMoveToPositionTween(playerPositions[0]);
            player.MoveToPositionTween.Play();
            yield return player.MoveToPositionTween.WaitForCompletion();
        }
    }
}
