﻿using System;
using CodeBase.ECSCore;
using CodeBase.Game.ComponentsUi;
using CodeBase.Infrastructure.Models;
using UniRx;

namespace CodeBase.Game.SystemsUi
{
    public sealed class SLevelTimeLeft : SystemComponent<CLevelTimeLeft>
    {
        private readonly LevelModel _levelModel;

        public SLevelTimeLeft(LevelModel levelModel)
        {
            _levelModel = levelModel;
        }

        protected override void OnEnableComponent(CLevelTimeLeft component)
        {
            base.OnEnableComponent(component);

            SubscribeOnUpdateTimeLeft(component);
        }

        private void SubscribeOnUpdateTimeLeft(CLevelTimeLeft component)
        {
            int time = _levelModel.Level.LevelTime;

            Observable.Timer(Time())
                .Repeat()
                .Where(_ => time > 0)
                .DoOnSubscribe(() => component.SetTimeLeftText(time))
                .Subscribe(_ => UpdateTime(component, ref time))
                .AddTo(component.LifetimeDisposable);
        }

        private TimeSpan Time() => TimeSpan.FromSeconds(1f);

        private void UpdateTime(CLevelTimeLeft component, ref int time)
        {
            time -= 1;
            component.SetTimeLeftText(time);
        }
    }
}