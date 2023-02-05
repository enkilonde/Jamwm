using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomPlayerInputs : MonoBehaviour
{
    public CustomCharacterController characterController;

    private Vector2 move;
    public Vector2 turn;
    public Vector3 mousePosition;

    public bool isUsingController = false;

    private Camera mainCamera;
    Plane ground;

    private void Awake()
    {
        mainCamera = Camera.main;
        ground = new Plane(Vector3.up, Vector3.zero);

    }

    private void Update()
    {
        if (!isUsingController)
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            float mousePosGround = 0;
            ground.Raycast(ray, out mousePosGround);
            Vector3 diff = ray.GetPoint(mousePosGround) - transform.position;
            turn = new Vector2(diff.x, diff.z);
        }


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

        if (context.started)
        {
            if (TempLootUi.Instance.CurrentItem != null)
                TempLootUi.Instance.PickupLeft();
            else
                characterController.StartChargeAttackLeft();
        }
        else if (context.canceled) characterController.LaunchAttackLeft();
    }

    public void AttackRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (TempLootUi.Instance.CurrentItem != null)
            TempLootUi.Instance.PickupRight();
            else
                characterController.StartChargeAttackRight();
            
        }
        else if (context.canceled) characterController.LaunchAttackRight();
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        characterController.Dash();
    }

    public void ActivateController(InputAction.CallbackContext context)
    {
        isUsingController = true;
    }

    public void Activatemouse(InputAction.CallbackContext context)
    {
        isUsingController = false;
        mousePosition = context.ReadValue<Vector2>();
        mousePosition.z = 1;
    }
}
