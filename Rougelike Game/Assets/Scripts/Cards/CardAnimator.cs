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
        private IPositionableObject positionableObject;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            originalSizeDelta = rect.sizeDelta;
            originalScale = rect.localScale;

            gameManager = GameManager.Instance;

            TryGetComponent<IPositionableObject>(out positionableObject);
        }

        /// <summary>
        /// Play card move to given position. Can invoke method when completed
        /// </summary>
        /// <param name="card"></param>
        /// <param name="targetPosition"></param>
        /// <param name="animationEase"></param>
        /// <param name="onComplete"></param>
        public void AnimateCardMove(GameObject card, Vector3 targetPosition, Ease animationEase = Ease.Linear, Action onComplete = null)
            => StartCoroutine(AnimateMove(card, targetPosition, animationEase, onComplete));

        /// <summary>
        /// Move card to given position using DOTween library
        /// </summary>
        /// <param name="card"></param>
        /// <param name="targetPosition"></param>
        /// <param name="animationEase"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
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

            var cards = transform.parent.GetComponentsInChildren<IPositionableObject>().ToList();
            int thisCardIndex = cards.IndexOf(gameObject.GetComponent<IPositionableObject>());

            UpdateCardsPosition(positionableObject, thisCardIndex);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            AnimateScale();

            UpdateCardsPosition(positionableObject);
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

        private void UpdateCardsPosition(IPositionableObject positionableObject, int? thisCardIndex = null)
        {
            gameManager.HandCardsPositioner.PositionObjects(scaledObjectIndex: thisCardIndex);
        }
    }
}
