using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private Vector2 ledgeForward;

    private readonly int HangingHash = Animator.StringToHash("Hanging");

    private float CrossFadeDuration = 0.1f;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward) : base(stateMachine)
    {
        this.ledgeForward = ledgeForward;
    }

    public override void Enter()
    {
        stateMachine.InputReader.DropEvent += OnDrop;

        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);

        stateMachine.Animator.CrossFadeInFixedTime(HangingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        stateMachine.InputReader.DropEvent -= OnDrop;
    }

    private void OnDrop()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
        stateMachine.SwitchState(new PlayerFallingState(stateMachine));
    }
}
    
