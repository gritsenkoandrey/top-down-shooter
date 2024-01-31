﻿using CodeBase.ECSCore;
using CodeBase.Game.ComponentsUi;
using CodeBase.Infrastructure.Progress;
using VContainer;

namespace CodeBase.Game.SystemsUi
{
    public sealed class SCurrentLevel : SystemComponent<CCurrentLevel>
    {
        private IProgressService _progressService;

        [Inject]
        public void Construct(IProgressService progressService)
        {
            _progressService = progressService;
        }

        protected override void OnEnableComponent(CCurrentLevel component)
        {
            base.OnEnableComponent(component);

            component.TextLevel.SetText("Level {0}", _progressService.LevelData.Data.Value);
        }
    }
}