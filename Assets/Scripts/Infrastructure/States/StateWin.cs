﻿using System;
using CodeBase.App;
using CodeBase.Infrastructure.Factories.UI;
using CodeBase.Infrastructure.Progress;
using CodeBase.UI.Screens;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;

namespace CodeBase.Infrastructure.States
{
    public sealed class StateWin : IEnterState
    {
        private readonly IGameStateService _stateService;
        private IUIFactory _uiFactory;
        private IProgressService _progressService;
        
        private IDisposable _transitionDisposable;

        public StateWin(IGameStateService stateService)
        {
            _stateService = stateService;
        }
        
        [Inject]
        private void Construct(IUIFactory uiFactory, IProgressService progressService)
        {
            _uiFactory = uiFactory;
            _progressService = progressService;
        }

        void IEnterState.Enter()
        {
            _progressService.LevelData.Data.Value++;
            
            SubscribeOnTransition().Forget();
        }

        void IExitState.Exit()
        {
            _transitionDisposable?.Dispose();
        }
        
        private async UniTaskVoid SubscribeOnTransition()
        {
            BaseScreen screen = await _uiFactory.CreateScreen(ScreenType.Win);

            _transitionDisposable = screen.ChangeState.First().Subscribe(ChangeState);
        }

        private void ChangeState(Unit _) => _stateService.Enter<StatePreview, string>(SceneName.Preview);
    }
}