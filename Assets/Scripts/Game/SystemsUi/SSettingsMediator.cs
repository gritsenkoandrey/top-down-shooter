﻿using CodeBase.ECSCore;
using CodeBase.Game.ComponentsUi;
using CodeBase.Infrastructure.Progress;
using UniRx;
using VContainer;

namespace CodeBase.Game.SystemsUi
{
    public sealed class SSettingsMediator : SystemComponent<CSettingsMediator>
    {
        private IProgressService _progressService;

        [Inject]
        private void Construct(IProgressService progressService)
        {
            _progressService = progressService;
        }
        
        protected override void OnEnableComponent(CSettingsMediator component)
        {
            base.OnEnableComponent(component);

            Init(component);
            
            component.VibrationToggle.IsEnable
                .Subscribe(SetHapticData)
                .AddTo(component.LifetimeDisposable);
        }

        private void Init(CSettingsMediator component) =>
            component.VibrationToggle.IsEnable.Value = _progressService.HapticData.Data.Value;
        
        private void SetHapticData(bool isEnable) => _progressService.HapticData.Data.Value = isEnable;
    }
}