﻿using CodeBase.ECSCore;
using CodeBase.Game.Components;
using CodeBase.Game.Interfaces;
using CodeBase.Infrastructure.Factories.Game;
using UniRx;

namespace CodeBase.Game.Systems
{
    public sealed class SCharacterWeapon : SystemComponent<CWeapon>
    {
        private readonly IGameFactory _gameFactory;

        public SCharacterWeapon(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }
        
        protected override void OnEnableSystem()
        {
            base.OnEnableSystem();
        }

        protected override void OnDisableSystem()
        {
            base.OnDisableSystem();
        }

        protected override void OnEnableComponent(CWeapon component)
        {
            base.OnEnableComponent(component);

            component.Shoot
                .Subscribe(_ =>
                {
                    IBullet bullet = _gameFactory.CreateBullet(component.SpawnBulletPoint.position);

                    bullet.Damage = component.Damage;
                    bullet.Direction = component.transform.forward.normalized * component.Force;
                })
                .AddTo(component.LifetimeDisposable);
        }

        protected override void OnDisableComponent(CWeapon component)
        {
            base.OnDisableComponent(component);
        }
    }
}