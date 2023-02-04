using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomPlayerInputs : MonoBehaviour
{
    public CustomCharacterController characterController;
    public void Move(InputAction.CallbackContext context)
    {
        characterController.Move(context.ReadValue<Vector2>());
    }

    public void Turn(InputAction.CallbackContext context)
    {
        characterController.Turn(context.ReadValue<Vector2>());
    }

    public void AttackLeft(InputAction.CallbackContext context)
    {
        if (context.started) characterController.ChargeAttackLeft();
        else if (context.canceled) characterController.LaunchAttackLeft();
    }

    public void AttackRight(InputAction.CallbackContext context)
    {
        if (context.started) characterController.ChargeAttackRight();
        else if (context.canceled) characterController.LaunchAttackRight();
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        characterController.Dodge();
    }
}
