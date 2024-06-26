﻿using System;
using CodeBase.Infrastructure.AppSettingsService;
using CodeBase.Infrastructure.AssetData;
using CodeBase.Infrastructure.CameraMain;
using CodeBase.Infrastructure.CheatService;
using CodeBase.Infrastructure.Curtain;
using CodeBase.Infrastructure.DailyTasks;
using CodeBase.Infrastructure.Factories.Effects;
using CodeBase.Infrastructure.Factories.Game;
using CodeBase.Infrastructure.Factories.StateMachine;
using CodeBase.Infrastructure.Factories.Systems;
using CodeBase.Infrastructure.Factories.TextureArray;
using CodeBase.Infrastructure.Factories.UI;
using CodeBase.Infrastructure.Factories.Weapon;
using CodeBase.Infrastructure.GUI;
using CodeBase.Infrastructure.Haptic;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.Loader;
using CodeBase.Infrastructure.Loot;
using CodeBase.Infrastructure.Models;
using CodeBase.Infrastructure.Pool;
using CodeBase.Infrastructure.Progress;
using CodeBase.Infrastructure.States;
using CodeBase.Infrastructure.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.LifeTime.Scopes
{
    public sealed class BootstrapScope : LifetimeScope
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private GuiService _guiService;
        [SerializeField] private JoystickService _joystickService;
        
        protected override void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(this);
        }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            void CreateGameStateMachine(IObjectResolver container) => 
                container.Resolve<IStateMachineFactory>().CreateGameStateMachine().Enter<StateBootstrap>();

            builder.RegisterComponentInNewPrefab(_loadingCurtain, Lifetime.Singleton).UnderTransform(transform).As<ILoadingCurtainService>();
            builder.RegisterComponentInNewPrefab(_cameraService, Lifetime.Singleton).UnderTransform(transform).As<ICameraService>();
            builder.RegisterComponentInNewPrefab(_guiService, Lifetime.Singleton).UnderTransform(transform).As<IGuiService>();
            builder.RegisterComponentInNewPrefab(_joystickService, Lifetime.Singleton).UnderTransform(transform).As<IJoystickService>();

            builder.Register<InventoryModel>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelModel>(Lifetime.Singleton).AsSelf();
            
            builder.Register<ILootService, LootService>(Lifetime.Singleton);
            builder.Register<IHapticService, HapticService>(Lifetime.Singleton);
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
            builder.Register<IProgressService, ProgressService>(Lifetime.Singleton);
            builder.Register<IAssetService, AssetService>(Lifetime.Singleton);
            builder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);
            builder.Register<IDailyTaskService, DailyTaskService>(Lifetime.Singleton);
            builder.Register<ICheatService, CheatService>(Lifetime.Singleton);
            builder.Register<IAppSettingsService, AppSettingsService>(Lifetime.Singleton);
            
            builder.Register<IStateMachineFactory, StateMachineFactory>(Lifetime.Singleton);
            builder.Register<ITextureArrayFactory, TextureArrayFactory>(Lifetime.Singleton);
            builder.Register<IGameFactory, GameFactory>(Lifetime.Singleton);
            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);
            builder.Register<ISystemFactory, SystemFactory>(Lifetime.Singleton);
            builder.Register<IEffectFactory, EffectFactory>(Lifetime.Singleton);
            builder.Register<IWeaponFactory, WeaponFactory>(Lifetime.Singleton);

            builder.Register<IObjectPoolService, ObjectPoolService>(Lifetime.Singleton).WithParameter(transform).As<IDisposable>();
            
            builder.RegisterBuildCallback(CreateGameStateMachine);
        }
    }
} 