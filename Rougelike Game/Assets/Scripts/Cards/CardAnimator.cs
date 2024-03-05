using DG.Tweening;
using Managers;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class CardAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float baseSpeed = 5000f;
        [SerializeField] private float minDuration = 0.1f, maxDuration = 0.4f;

        [SerializeField] RectTransform frame;
        private RectTransform rect;
        private Vector2 originalSizeDelta;
        private Vector3 originalScale;

        private GameManager gameManager;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            originalSizeDelta = rect.sizeDelta;
            originalScale = rect.localScale;

            gameManager = GameManager.Instance;
        }

        public void AnimateCardMove(GameObject card, Vector3 targetPosition, Ease animationEase = Ease.Linear, Action onComplete = null)
            => StartCoroutine(AnimateMove(card, targetPosition, animationEase, onComplete));

        private IEnumerator AnimateMove(GameObject card, Vector3 targetPosition, Ease animationEase, Action onComplete)
        {
            float disrance = Vector3.Distance(card.transform.position, targetPosition);
            float duration = disrance / baseSpeed;
            duration = Mathf.Clamp(duration, minDuration, maxDuration);

            Tween moveTween = card.transform.DOMove(targetPosition, duration).SetEase(animationEase);
            yield return moveTween.WaitForCompletion();

            onComplete?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AnimateScale(gameManager.HandPositioningConfig.ScaleMultiplier);

            var cards = transform.parent.GetComponentsInChildren<ICardRect>().ToList();
            int thisCardIndex = cards.IndexOf(gameObject.GetComponent<ICardRect>());

            UpdateCardsPosition(thisCardIndex);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            AnimateScale();
            UpdateCardsPosition();
        }

        private void AnimateScale(float multiplier = 1f, float duration = 0.2f)
        {
            if (frame == null)
            {
                Debug.LogWarning("Object need content frame object.");
                return;
            }

            rect.DOSizeDelta(originalSizeDelta * multiplier, duration);
            frame.DOScale(originalScale * multiplier, duration);
        }

        private void UpdateCardsPosition(int? thisCardIndex = null)
        {
            GameManager.Instance.HandCardsPositioner.PositionCards(originalSizeDelta.x, thisCardIndex);
        }
    }
}
