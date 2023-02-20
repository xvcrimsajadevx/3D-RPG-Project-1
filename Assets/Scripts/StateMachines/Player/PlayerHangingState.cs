using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private Vector3 ledgeForward;
    private Vector3 closestPoint;

    private readonly int HangingHash = Animator.StringToHash("Hanging");

    private float CrossFadeDuration = 0.1f;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter()
    {
        stateMachine.InputReader.DropEvent += OnDrop;
        stateMachine.InputReader.PullUpEvent += OnPullUp;

        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);

        stateMachine.Controller.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.Controller.enabled = true;

        stateMachine.Animator.CrossFadeInFixedTime(HangingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        stateMachine.InputReader.DropEvent -= OnDrop;
        stateMachine.InputReader.PullUpEvent -= OnPullUp;
    }

    private void OnDrop()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
        stateMachine.SwitchState(new PlayerFallingState(stateMachine));
    }

    private void OnPullUp()
    {
        stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
    }
}
    
