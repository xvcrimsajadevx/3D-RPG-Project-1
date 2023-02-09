using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");

    private float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;

        if (duration <= 0)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit() { }

    
}
