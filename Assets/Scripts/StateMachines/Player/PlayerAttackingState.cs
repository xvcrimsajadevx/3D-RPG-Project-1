using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackId) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackId];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }

}
