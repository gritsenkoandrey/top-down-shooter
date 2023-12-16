﻿using CodeBase.ECSCore;
using CodeBase.Game.Components;

namespace CodeBase.Game.StateMachine.Zombie
{
    public sealed class ZombieStateNone : ZombieState, IState
    {
        public ZombieStateNone(IStateMachine stateMachine, CZombie zombie) : base(stateMachine, zombie) { }

        void IState.Enter()
        {
            Zombie.Agent.ResetPath();
            Zombie.Radar.Clear.Execute();
            Zombie.Animator.OnIdle.Execute();
            Zombie.CleanSubscribe();
        }

        void IState.Exit() { }

        void IState.Tick() { }
    }
}