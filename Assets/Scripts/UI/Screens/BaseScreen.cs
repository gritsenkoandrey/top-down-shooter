﻿using CodeBase.Infrastructure.Factories.UI;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.UI.Screens
{
    public abstract class BaseScreen : MonoBehaviour
    {
        protected IUIFactory UIFactory { get; private set; }
        protected IGameStateService GameStateService { get; private set; }

        public virtual void Construct(IUIFactory uiFactory, IGameStateService gameStateService)
        {
            UIFactory = uiFactory;
            GameStateService = gameStateService;
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
    }
}