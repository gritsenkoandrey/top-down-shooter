﻿using CodeBase.Game.Components;
using CodeBase.Game.Enums;
using CodeBase.Game.Interfaces;
using CodeBase.Infrastructure.StaticData.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories.Weapon
{
    public interface IWeaponFactory
    {
        UniTask<CWeapon> CreateCharacterWeapon(WeaponType type, Transform parent);
        UniTask<CWeapon> CreateUnitWeapon(WeaponType type, WeaponCharacteristic weaponCharacteristic, Transform parent);
        IWeapon CreateTurretWeapon(CWeapon weapon, WeaponCharacteristic weaponCharacteristic);
        UniTask<IProjectile> CreateProjectile(ProjectileType type, Transform spawnPoint, int damage, Vector3 direction);
    }
}