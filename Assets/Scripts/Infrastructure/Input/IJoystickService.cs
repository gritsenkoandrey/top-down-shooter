﻿using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Input
{
    public interface IJoystickService : IService
    {
        public Vector2 GetAxis();
        public void Init();
        public void Enable(bool isEnable);
        public void Execute();
    }
}