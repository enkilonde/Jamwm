using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomPlayerInputs : MonoBehaviour
{
    public CustomCharacterController characterController;

    private Vector2 move;
    private Vector2 turn;

    private void Update()
    {
        characterController.Move(move);
        characterController.Turn(turn);
    }


    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void Turn(InputAction.CallbackContext context)
    {
        turn = context.ReadValue<Vector2>();
    }

    public void AttackLeft(InputAction.CallbackContext context)
    {
        if (context.started) characterController.StartChargeAttackLeft();
        else if (context.canceled) characterController.LaunchAttackLeft();
    }

    public void AttackRight(InputAction.CallbackContext context)
    {
        if (context.started) characterController.StartChargeAttackRight();
        else if (context.canceled) characterController.LaunchAttackRight();
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        characterController.Dash();
    }
}
