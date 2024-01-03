﻿using CodeBase.ECSCore;
using CodeBase.Game.Components;
using CodeBase.Game.Interfaces;
using CodeBase.Game.StateMachine.Character;
using CodeBase.Infrastructure.CameraMain;
using CodeBase.Infrastructure.Factories.Game;
using CodeBase.Infrastructure.Factories.UI;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.Models;
using CodeBase.Infrastructure.Progress;
using CodeBase.Utils;
using Cysharp.Threading.Tasks;
using UniRx;

namespace CodeBase.Game.Systems
{
    public sealed class SCharacterSpawner : SystemComponent<CCharacterSpawner>
    {
        private readonly IGameFactory _gameFactory;
        private readonly IProgressService _progressService;
        private readonly IUIFactory _uiFactory;
        private readonly ICameraService _cameraService;
        private readonly IJoystickService _joystickService;
        private readonly LevelModel _levelModel;

        public SCharacterSpawner(IGameFactory gameFactory, IProgressService progressService, IUIFactory uiFactory, 
            ICameraService cameraService, IJoystickService joystickService, LevelModel levelModel)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
            _uiFactory = uiFactory;
            _cameraService = cameraService;
            _joystickService = joystickService;
            _levelModel = levelModel;
        }
        
        protected override void OnEnableComponent(CCharacterSpawner component)
        {
            base.OnEnableComponent(component);
            
            CreateCharacter(component).Forget();
        }

        private async UniTaskVoid CreateCharacter(CCharacterSpawner component)
        {
            ICharacter character = await _gameFactory.CreateCharacter(component.Position, component.transform.parent);

            ReadProgress();
            CreateStateMachine(character);
        }

        private void ReadProgress()
        {
            _uiFactory.ProgressReaders.Foreach(ReadProgress);
            _gameFactory.ProgressReaders.Foreach(ReadProgress);
        }

        private void ReadProgress(IProgressReader progress)
        {
            progress.Read(_progressService.PlayerProgress);
        }

        private void CreateStateMachine(ICharacter character)
        {
            character.StateMachine.SetStateMachine(new CharacterStateMachine(character, _cameraService, _joystickService, _levelModel));

            character.StateMachine.UpdateStateMachine
                .Subscribe(_ => character.StateMachine.StateMachine.Tick())
                .AddTo(character.Entity.LifetimeDisposable);
        }
    }
}