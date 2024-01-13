﻿using System;
using CodeBase.Infrastructure.Factories.UI;
using CodeBase.UI.Screens;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UniRx;
using VContainer.Unity;

namespace CodeBase.Infrastructure.States
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class StateLobby : IEnterState
    {
        private readonly IGameStateService _stateService;
        private readonly IUIFactory _uiFactory;

        private IDisposable _transitionDisposable;

        public StateLobby(IGameStateService stateService, IUIFactory uiFactory)
        {
            _stateService = stateService;
            _uiFactory = uiFactory;
        }
        
        void IInitializable.Initialize() => _stateService.AddState(this);

        void IEnterState.Enter()
        {
            SubscribeOnTransition().Forget();
        }

        void IExitState.Exit()
        {
            _transitionDisposable?.Dispose();
        }
        
        private async UniTaskVoid SubscribeOnTransition()
        {
            BaseScreen screen = await _uiFactory.CreateScreen(ScreenType.Lobby);

            _transitionDisposable = screen.ChangeState.First().Subscribe(ChangeState);
        }

        private void ChangeState(Unit _) => _stateService.Enter<StateGame>();
    }
}