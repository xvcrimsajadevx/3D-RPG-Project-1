using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    // ==================== String to Hash ====================
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private float CrossFadeDuration = 0.1f;


    // ==================== Constructor/Base Methods ====================
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        
        stateMachine.InputReader.TargetEvent += OnTargetPressed;
        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.InputReader.DodgeEvent += OnDodge;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateTargetMovement();

        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTargetPressed;
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }


    // ==================== Movement Methods ====================
    private Vector3 CalculateTargetMovement()
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else {
            float value = stateMachine.InputReader.MovementValue.y;
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else {
            float value = stateMachine.InputReader.MovementValue.x;
            stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }
    }


    // ==================== Switch State Methods ====================
    private void OnTargetPressed()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        return;
    }

    private void OnAttack()
    {
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        return;
    }

    private void OnDodge()
    {
        if (Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCoolDown) { return; }
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine));
        return;
    }
}
