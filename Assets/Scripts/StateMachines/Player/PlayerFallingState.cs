using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallingHash = Animator.StringToHash("Falling");

    private float CrossFadeDuration = 0.1f;

    private Vector3 momentum;


    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(FallingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }

    public override void Exit() { }

}