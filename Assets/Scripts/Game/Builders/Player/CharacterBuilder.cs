﻿using CodeBase.Game.Components;
using CodeBase.Infrastructure.CameraMain;
using UnityEngine;

namespace CodeBase.Game.Builders.Player
{
    public sealed class CharacterBuilder
    {
        private GameObject _prefab;
        private ICameraService _cameraService;

        private Transform _parent;
        private Vector3 _position;
        private int _health;
        private float _speed;

        public CharacterBuilder SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
            return this;
        }

        public CharacterBuilder SetParent(Transform parent)
        {
            _parent = parent;
            return this;
        }

        public CharacterBuilder SetPosition(Vector3 position)
        {
            _position = position;
            return this;
        }

        public CharacterBuilder SetHealth(int health)
        {
            _health = health;
            return this;
        }
        
        public CharacterBuilder SetSpeed(float speed)
        {
            _speed = speed;
            return this;
        }

        public CharacterBuilder SetCamera(ICameraService cameraService)
        {
            _cameraService = cameraService;
            return this;
        }

        public CCharacter Build()
        {
            CCharacter character = Object.Instantiate(_prefab, _position, Quaternion.identity, _parent)
                .GetComponent<CCharacter>();

            character.Health.SetBaseHealth(_health);
            character.Move.SetBaseSpeed(_speed);
            
            _cameraService.SetTarget(character.transform);

            return character;
        }
    }
}