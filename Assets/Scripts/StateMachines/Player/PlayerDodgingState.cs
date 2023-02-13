using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgingForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgingRight");
    private float CrossFadeDuration = 0.1f;

    private Vector2 dodgeDirectionInput;
    private float remainingDodgeTime;
    private float dodgeLengthRemaining;
    
    public PlayerDodgingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        

        dodgeDirectionInput = stateMachine.InputReader.MovementValue;
        stateMachine.SetDodgeTime(Time.time);

        remainingDodgeTime = stateMachine.DodgeDuration;
        dodgeLengthRemaining = stateMachine.DodgeLength;
        
        stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        if (remainingDodgeTime > 0f)
        {
            movement += stateMachine.transform.right * dodgeDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
            movement += stateMachine.transform.forward * dodgeDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;
            
            remainingDodgeTime = Mathf.Max(remainingDodgeTime - deltaTime, 0f);
        }
        else
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }

        FaceTarget();

        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);
    }

    public override void Exit()
    {
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(DodgeForwardHash, 0, 0.1f, deltaTime);
        }
        else {
            float value = stateMachine.InputReader.MovementValue.y;
            stateMachine.Animator.SetFloat(DodgeForwardHash, value, 0.1f, deltaTime);
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(DodgeRightHash, 0, 0.1f, deltaTime);
        }
        else {
            float value = stateMachine.InputReader.MovementValue.x;
            stateMachine.Animator.SetFloat(DodgeRightHash, value, 0.1f, deltaTime);
        }
    }
    
}
