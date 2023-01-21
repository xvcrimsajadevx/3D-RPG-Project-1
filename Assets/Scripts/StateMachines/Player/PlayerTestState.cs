using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();
        
        movement.x = stateMachine.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = stateMachine.InputReader.MovementValue.y;

        stateMachine.transform.Translate(movement * deltaTime);
    }

    public override void Exit()
    {
    }

}
