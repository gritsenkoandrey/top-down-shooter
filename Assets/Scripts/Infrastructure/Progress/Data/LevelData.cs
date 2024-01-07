﻿using System;
using CodeBase.Infrastructure.SaveLoad;
using UniRx;
using UnityEngine;

namespace CodeBase.Infrastructure.Progress.Data
{
    public sealed class LevelData : ISaveLoad<int>
    {
        private readonly IDisposable _disposable;
        
        public IReactiveProperty<int> Data { get; }

        public LevelData()
        {
            Data = new ReactiveProperty<int>(Load());

            _disposable = Data.Subscribe(Save);
        }

        public void Save(int data)
        {
            PlayerPrefs.SetInt(DataKeys.Level, data);
            PlayerPrefs.Save();
        }

        public int Load() => PlayerPrefs.GetInt(DataKeys.Level, 1);

        public void Dispose() => _disposable?.Dispose();
    }
}