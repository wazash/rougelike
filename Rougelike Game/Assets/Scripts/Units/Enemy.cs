using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace Units
{
    public class Enemy : Unit
    {
        [InlineEditor]
        [SerializeField] private EnemyData enemyData;

        private Tween idleTween;
        private Tween moveToPositionTween;

        public EnemyData EnemyData => enemyData;
        public Tween IdleTween => idleTween;
        public Tween MoveToPositionTween => moveToPositionTween;

        protected override void Start()
        {
            base.Start();

            SetIdleTween();

            healthComponent.OnDeath += OnDeath;
        }

        private void OnDeath()
        {
            IdleTween.Kill();
        }

        public void SetEnemyData(EnemyData enemyData)
        {
            this.enemyData = enemyData;
        }

        public void SetMoveToPositionTween(Transform groundPosition)
        {
            moveToPositionTween = transform.DOMove(groundPosition.position, .5f).SetEase(Ease.InOutSine);
            moveToPositionTween.Pause();
        }

        private void SetIdleTween()
        {
            transform.DORotate(new Vector3(0, 0, -5), 0f);
            idleTween = transform.DORotate(new Vector3(0, 0, 5), 3.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            idleTween.Pause();
        }
    }
}
