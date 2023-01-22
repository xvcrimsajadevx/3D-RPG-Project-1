using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTargetPressed;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent += OnTargetPressed;
    }

    private void OnTargetPressed()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        Debug.Log("Entering Free Look State");
    }
    
}
