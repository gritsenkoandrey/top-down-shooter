﻿using UnityEngine;

namespace CodeBase.Infrastructure.AssetData.Data
{
    [CreateAssetMenu(fileName = "UiAssetData", menuName = "Data/UiAssetData", order = 0)]
    public sealed class UiAssetData : ScriptableObject
    {
        public GameObject Canvas;
    }
}