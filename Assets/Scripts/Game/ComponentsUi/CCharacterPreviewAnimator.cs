﻿using CodeBase.ECSCore;
using CodeBase.Game.Behaviours.AnimationStateBehaviour;
using CodeBase.Utils;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.ComponentsUi
{
    [RequireComponent(typeof(Animator))]
    public sealed class CCharacterPreviewAnimator : EntityComponent<CCharacterPreviewAnimator>, IAnimationStateReader
    {
        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;
        
        public readonly ReactiveCommand StartAnimation = new();
        public bool IsExitAnimation { get; private set; }

        void IAnimationStateReader.EnteredState(int stateHash) { }

        void IAnimationStateReader.ExitedState(int stateHash)
        {
            if (Animations.StartGame == stateHash)
            {
                IsExitAnimation = true;
            }
        }
    }
}