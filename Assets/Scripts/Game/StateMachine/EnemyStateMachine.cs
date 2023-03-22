﻿using System;
using System.Collections.Generic;
using CodeBase.Game.Components;
using CodeBase.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Game.StateMachine
{
    public sealed class EnemyStateMachine
    {
        private readonly CEnemy _enemy;

        private Dictionary<State, Action> _actions;
        
        private State _state;
        
        private Vector3 _patrolPosition;
        private float _maxDelay;
        private float _delay;
        private float _startDelay;
        private float _minDistance;
        private float _patrolRadius;
        private float _pursuitDistance;
        private float _normalSpeed;
        private float _pursuitSpeed;
        private float _attackDelay;
        private float _maxAttackDelay;

        public EnemyStateMachine(CEnemy enemy)
        {
            _enemy = enemy;
        }

        public void Init()
        {
            _state = State.Idle;
            _patrolPosition = _enemy.transform.position;
            _maxDelay = _enemy.Stats.StayDelay;
            _delay = _maxDelay;
            _startDelay = _enemy.Stats.AliveDelay;
            _minDistance = _enemy.Stats.MinDistanceToTarget;
            _patrolRadius = _enemy.Stats.PatrolRadius;
            _pursuitDistance = _enemy.Stats.PursuitRadius;
            _normalSpeed = _enemy.Stats.WalkSpeed;
            _pursuitSpeed = _enemy.Stats.RunSpeed;
            _maxAttackDelay = _enemy.Stats.AttackDelay;
            _attackDelay = _maxAttackDelay;
            _enemy.Radar.Radius = _enemy.Stats.AggroRadius;

            _actions = new Dictionary<State, Action>
            {
                { State.Idle, Idle },
                { State.Patrol, Patrol },
                { State.Pursuit, Pursuit },
            };
        }

        public void Tick()
        {
            _actions[_state].Invoke();
        }

        private void Idle()
        {
            if (_startDelay > 0f)
            {
                _startDelay -= Time.deltaTime;
            }
            else
            {
                if (Distance() < _enemy.Radar.Radius || _enemy.IsAggro)
                {
                    PursuitState();
                }
                else
                {
                    PatrolState();
                }
            }
        }

        private void Patrol()
        {
            if (Distance() < _enemy.Radar.Radius || _enemy.IsAggro)
            {
                PursuitState();
            }
            else
            {
                if (_enemy.Agent.hasPath)
                {
                    return;
                }

                if (_delay > 0f)
                {
                    _delay -= Time.deltaTime;
                }
                else
                {
                    _delay = _maxDelay;
                    _enemy.Agent.SetDestination(GeneratePointOnNavmesh());
                }
            }
        }

        private void Pursuit()
        {
            if (Distance() > _pursuitDistance)
            {
                _enemy.IsAggro = false;
                
                PatrolState();
            }
            else
            {
                LockAt();
                
                if (Distance() < _minDistance)
                {
                    if (_enemy.Agent.hasPath)
                    {
                        _enemy.Agent.ResetPath();
                    }
                    
                    Attack();
                }
                else
                {
                    _enemy.Agent.SetDestination(_enemy.Character.Position);
                }

                _attackDelay -= Time.deltaTime;
            }
        }

        private void Attack()
        {
            if (_attackDelay < 0f)
            {
                _attackDelay = _maxAttackDelay;

                _enemy.Melee.Attack.Execute();
            }
        }

        private void LockAt()
        {
            Quaternion lookRotation = Quaternion.LookRotation(_enemy.Character.Position - _enemy.Position);

            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, lookRotation, 0.5f);
        }

        private Vector3 GeneratePointOnNavmesh()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 center = _patrolPosition + GenerateRandomPoint(_patrolRadius);

                if (NavMesh.SamplePosition(center, out NavMeshHit hit, 1f, 1))
                {
                    return hit.position;
                }
            }
            
            return Vector3.zero;
        }
        
        private Vector3 GenerateRandomPoint(float radius)
        {
            float angle = UnityEngine.Random.Range(0f, 1f) * (2f * Mathf.PI) - Mathf.PI;
                    
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            return new Vector3(x, 0f, z);
        }

        private float Distance() => Vector3.Distance(_enemy.Position, _enemy.Character.Position);

        private void PursuitState()
        {
            _enemy.Agent.ResetPath();
            _enemy.Agent.speed = _pursuitSpeed;
            _enemy.Animator.Animator.SetFloat(Animations.RunBlend, 1f);
            _enemy.Radar.Clear.Execute();
            _state = State.Pursuit;
        }

        private void PatrolState()
        {
            _enemy.Agent.ResetPath();
            _enemy.Agent.speed = _normalSpeed;
            _enemy.Animator.Animator.SetFloat(Animations.RunBlend, 0f);
            _enemy.Radar.Draw.Execute();
            _state = State.Patrol;
        }
    }
}