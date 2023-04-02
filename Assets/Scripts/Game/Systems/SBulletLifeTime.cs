﻿using CodeBase.ECSCore;
using CodeBase.Game.Components;
using CodeBase.Infrastructure.Pool;
using DG.Tweening;

namespace CodeBase.Game.Systems
{
    public sealed class SBulletLifeTime : SystemComponent<CBullet>
    {
        private readonly IObjectPoolService _objectPoolService;

        public SBulletLifeTime(IObjectPoolService objectPoolService)
        {
            _objectPoolService = objectPoolService;
        }
        
        protected override void OnEnableSystem()
        {
            base.OnEnableSystem();
        }

        protected override void OnDisableSystem()
        {
            base.OnDisableSystem();
        }

        protected override void OnEnableComponent(CBullet component)
        {
            base.OnEnableComponent(component);

            component.Tween = DOVirtual.DelayedCall(2.5f, () =>
            {
                component.Rigidbody.isKinematic = true;
                
                _objectPoolService.ReleaseObject(component.Object);
            });
        }

        protected override void OnDisableComponent(CBullet component)
        {
            base.OnDisableComponent(component);
            
            component.Tween?.Kill();
        }
    }
}