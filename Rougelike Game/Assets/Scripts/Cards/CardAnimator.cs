using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

namespace Cards
{
    public class CardAnimator : MonoBehaviour
    {
        [SerializeField] private float baseSpeed = 5000f;
        [SerializeField] private float minDuration = 0.1f, maxDuration = 0.4f;

        public void AnimateCardMove(GameObject card, Transform targetPosition, Ease animationEase = Ease.Linear, Action onComplete = null) 
            => StartCoroutine(AnimateMove(card, targetPosition, animationEase, onComplete));

        private IEnumerator AnimateMove(GameObject card, Transform targetTransform, Ease animationEase, Action onComplete)
        {
            float disrance = Vector3.Distance(card.transform.position, targetTransform.position);
            float duration = disrance / baseSpeed;
            duration = Mathf.Clamp(duration, minDuration, maxDuration);

            Tween moveTween = card.transform.DOMove(targetTransform.position, duration).SetEase(animationEase);
            yield return moveTween.WaitForCompletion();

            onComplete?.Invoke();
        }
    }
}
