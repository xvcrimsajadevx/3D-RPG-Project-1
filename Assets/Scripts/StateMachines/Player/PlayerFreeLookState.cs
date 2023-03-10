using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private bool shouldFade;

    // ==================== String to Hash ====================
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");


    // ==================== State Variables ====================
    private const float AnimatorDampTime = 0.1f;


    // ==================== Constructor/Base Methods ====================
    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0);

        if (shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, 0.1f);
        }
        else
        {
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }
        
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }


    // ==================== Movement Methods ====================
    


    // ==================== Switch State Methods ====================
    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) { return; }
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        return;
    }

    private void OnAttack()
    {
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
        return;
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
        return;
    }
}
