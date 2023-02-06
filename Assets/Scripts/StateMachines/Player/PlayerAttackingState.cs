using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    // ==================== State Variables ====================
    private bool attackButtonPressed;
    private bool forceApplied;
    private bool windingApplied;
    private float previousFrameTime;
    private Attack attack;
    private Vector3 movement;


    // ==================== Constructor/Base Methods ====================
    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.InputReader.AttackEvent += OnAttackButtonPressed;

        movement = CalculateMovement();

        stateMachine.Weapon.SetAttack(attack.Damage);

        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        if (movement != Vector3.zero)
        {
            FaceMovementDirection(movement, deltaTime);
        }

        FaceTarget();

        float normalizedTime = GetNormalizedTime();

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attack.WindingTime && normalizedTime <= attack.ForceTime)
            {
                TryApplyWinding();
            }
            else if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if (attackButtonPressed)
            {
                TryComboAttack(normalizedTime);

                attackButtonPressed = false;
            }            
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.InputReader.AttackEvent -= OnAttackButtonPressed;
    }


    // ==================== Attack State Methods ====================
    private void OnAttackButtonPressed()
    {
        if (attackButtonPressed) { return; }

        attackButtonPressed = true;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }
        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachine,
                attack.ComboStateIndex
            )
        );
    }

    private void TryApplyWinding()
    {
        if (windingApplied) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * 15, attack.WindingDrag);

        forceApplied = true;
    }

    private void TryApplyForce()
    {
        if (forceApplied) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force, attack.Drag);

        forceApplied = true;
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }


    // ==================== Switch State Methods ====================
    

}