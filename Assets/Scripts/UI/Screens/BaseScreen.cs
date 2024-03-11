﻿using CodeBase.Utils;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Screens
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _safeArea;
        
        public readonly ReactiveCommand CloseScreen = new ();
        
        private protected readonly CompositeDisposable LifeTimeDisposable = new();

        private protected virtual void OnEnable() => _safeArea.ApplySafeArea();
        private protected virtual void OnDisable() => LifeTimeDisposable.Clear();
        
        public void SetActive(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1f : 0f;
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
        }

        private protected Tween FadeCanvas(float from, float to, float duration)
        {
            return _canvasGroup
                .DOFade(to, duration)
                .From(from)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        private protected Tween BounceButton(Button button, float to, float duration)
        {
            return button.transform
                .DOScale(Vector3.one * to, duration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        private protected void ActivateButton(Button button, bool isActive)
        {
            button.interactable = isActive;
            button.gameObject.SetActive(isActive);
        }
    }
}