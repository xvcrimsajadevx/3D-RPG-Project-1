using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    // ==================== String to Hash ====================
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");

    // ==================== Constructor/Base Methods ====================
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.Play(TargetingBlendTreeHash);
        stateMachine.InputReader.TargetEvent += OnTargetPressed;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent += OnTargetPressed;
    }

    // ==================== Switch State Methods ====================
    private void OnTargetPressed()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
    
    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }
}