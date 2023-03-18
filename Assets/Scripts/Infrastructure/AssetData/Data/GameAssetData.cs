﻿using CodeBase.Game.Components;
using CodeBase.Game.LevelData;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetData.Data
{
    [CreateAssetMenu(fileName = "GameAssetData", menuName = "Data/GameAssetData", order = 0)]
    public sealed class GameAssetData : ScriptableObject
    {
        public Level Level;
        public CBullet Bullet;
    }
}