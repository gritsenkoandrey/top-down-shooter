﻿using CodeBase.Game.Components;
using CodeBase.Utils;
using UnityEngine;

namespace CodeBase.Game.StateMachine.Zombie
{
    public sealed class ZombieStatePursuit : ZombieState, IEnemyState
    {
        private float _attackDelay;

        public ZombieStatePursuit(IEnemyStateMachine stateMachine, CZombie zombie) : base(stateMachine, zombie) { }
        
        void IEnemyState.Enter()
        {
            _attackDelay = Zombie.Stats.AttackDelay;
            Zombie.Agent.speed = Zombie.Stats.RunSpeed;
            Zombie.Animator.Animator.SetFloat(Animations.RunBlend, 1f);
            Zombie.Radar.Clear.Execute();
        }

        void IEnemyState.Exit()
        {
            Zombie.Agent.ResetPath();
            _attackDelay = Zombie.Stats.AttackDelay;
        }

        void IEnemyState.Tick()
        {
            if (Distance() > Zombie.Stats.PursuitRadius)
            {
                Zombie.IsAggro = false;
                
                StateMachine.Enter<ZombieStateIdle>();
            }
            else
            {
                LockAt();
                
                if (Distance() < Zombie.Stats.MinDistanceToTarget)
                {
                    if (Zombie.Agent.hasPath)
                    {
                        Zombie.Agent.ResetPath();
                    }
                    
                    Attack();
                }
                else
                {
                    Zombie.Agent.SetDestination(Zombie.Character.Position);
                }

                _attackDelay -= Time.deltaTime;
            }
        }
        
        private void Attack()
        {
            if (_attackDelay < 0f)
            {
                _attackDelay = Zombie.Stats.AttackDelay;

                Zombie.Melee.Attack.Execute();
            }
        }
        
        private void LockAt()
        {
            Quaternion lookRotation = Quaternion.LookRotation(Zombie.Character.Position - Zombie.Position);

            Zombie.transform.rotation = Quaternion.Slerp(Zombie.transform.rotation, lookRotation, 0.5f);
        }
        
        private float Distance() => Vector3.Distance(Zombie.Position, Zombie.Character.Position);
    }
}