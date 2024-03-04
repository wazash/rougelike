using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class CardAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float baseSpeed = 5000f;
        [SerializeField] private float minDuration = 0.1f, maxDuration = 0.4f;

        [SerializeField] RectTransform frame;
        [SerializeField] private float scaleMultiplier = 1.2f;
        private RectTransform rect;
        private Vector2 originalSizeDelta;
        private Vector3 originalScale;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            originalSizeDelta = rect.sizeDelta;
            originalScale = rect.localScale;
        }

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

        public void OnPointerEnter(PointerEventData eventData) => AnimateScale(scaleMultiplier);

        public void OnPointerExit(PointerEventData eventData) => AnimateScale();

        private void AnimateScale(float multiplier = 1f, float duration = 0.2f)
        {
            if(frame == null)
            {
                Debug.LogWarning("Object need content frame object.");
                return;
            }

            rect.DOSizeDelta(originalSizeDelta * multiplier, duration);
            frame.DOScale(originalScale * multiplier, duration);
        }
    }
}
