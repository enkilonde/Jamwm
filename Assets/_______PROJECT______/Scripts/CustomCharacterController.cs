using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomCharacterController : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public PlayerVisual playerVisual;

    [Header("Resources")]
    public ItemDatabase ItemDatabase;

    [Header("Movement")]
    public float moveSpeed;
    public float turnSpeed;

    public float dashSpeed;
    public float dashDistance;
    public float dashCooldown;

    private bool isDashing;
    private float dashTimer;
    private Vector3 lastMovingDirection;

    public List<float> movementPenalties = new List<float>();
    public PlayerSheet PlayerSheet;

    [Header("Combat")]
    public float attachChargeTime;

    private bool isChargingLeft;
    private bool isChargingRight;
    private float chargingLeft;
    private float chargingRight;



    private bool isLocked => isDashing; //may add more conditions

    private void Awake() {
        PlayerSheet = new PlayerSheet(ItemDatabase);
        dashTimer = dashCooldown;
    }

    private void Update()
    {
        ChargeAttackLeft();
        ChargeAttackRight();
    }


    public void Move(Vector2 direction)
    {
        if (isLocked) return;
        int attackPenalty = 0 + (isChargingLeft ? 1 : 0) + (isChargingRight ? 1 : 0);

        lastMovingDirection = new Vector3(direction.x, 0, direction.y);
        characterController.Move(lastMovingDirection * moveSpeed * Time.deltaTime * movementPenalties[attackPenalty]);
    }

    public void Turn(Vector2 vector2)
    {
        if (isLocked) return;

        Quaternion oldRot = characterController.transform.rotation;
        characterController.transform.LookAt(transform.position + new Vector3(vector2.x, 0, vector2.y));
        characterController.transform.rotation = Quaternion.Slerp(oldRot, characterController.transform.rotation, Time.deltaTime * turnSpeed);
    }    
    
    public void Dash()
    {
        if (isLocked) return;
        if (dashTimer < dashCooldown) return;
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        playerVisual.dashFX.Play();

        dashTimer = 0;
        Vector3 DashDirection = lastMovingDirection == Vector3.zero ? transform.forward : lastMovingDirection.normalized;
        Vector3 dashDestination = transform.position + DashDirection.normalized * dashDistance;
        float distance = Vector3.Distance(transform.position, dashDestination);

        while (dashTimer < dashCooldown)
        {
            dashTimer += Time.deltaTime;
            distance = Vector3.Distance(transform.position, dashDestination);
            Vector3 displacement = DashDirection * dashSpeed * Time.fixedDeltaTime;
            if(displacement.magnitude > distance)
            {
                characterController.Move(displacement.normalized * distance);
                break;
            }
            characterController.Move(displacement);
            
            yield return null;

        }

        playerVisual.dashFX.Stop();
        isDashing = false;

        while (dashTimer < dashCooldown)
        {
            dashTimer += Time.deltaTime;
            yield return null;
        }
    }

    public void StartChargeAttackLeft()
    {
        isChargingLeft = true;
    }

    private void ChargeAttackLeft()
    {
        if (!isChargingLeft) return;
        chargingLeft += Time.deltaTime;
    }

    public void LaunchAttackLeft()
    {
        if (!isChargingLeft) return;
    }

    public void StartChargeAttackRight()
    {
        isChargingRight = true;

    }

    private void ChargeAttackRight()
    {
        if (!isChargingRight) return;
        chargingRight += Time.deltaTime;
    }

    public void LaunchAttackRight()
    {
        if (!isChargingRight) return;

    }
}
