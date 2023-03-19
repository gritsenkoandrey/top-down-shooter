﻿using CodeBase.ECSCore;
using TMPro;
using UnityEngine;

namespace CodeBase.Game.Components
{
    public sealed class CHealthView : EntityComponent<CHealthView>
    {
        [SerializeField] private GameObject _background;
        [SerializeField] private Transform _fill;
        [SerializeField] private TextMeshPro _text;

        public GameObject Background => _background;
        public Transform Fill => _fill;
        public TextMeshPro Text => _text;
        
        protected override void OnEntityCreate() { }
        protected override void OnEntityEnable() { }
        protected override void OnEntityDisable() { }
    }
}