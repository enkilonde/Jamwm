using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeapoonChargeState
{
    StartCharging, 
    Charging,
    Release
}

public class CustomCharacterController : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public PlayerVisual playerVisual;
    public WeaponAction leftWeapon;
    public WeaponAction rightWeapon;

    [Header("Configuration")]
    public bool IsPlayer; // True for the Player ; False for the Bosses

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
    public CharacterSheet CharacterSheet;

    [Header("Combat")]
    public float attachChargeTime;

    private bool isChargingLeft;
    private bool isChargingRight;
    public float chargingLeft;
    private float chargingRight;



    private bool isLocked => isDashing; //may add more conditions

    private void Awake() {
        if (IsPlayer) {
            CharacterSheet = new PlayerSheet(playerVisual, this.transform);
        }
        dashTimer = dashCooldown;

    }

    private void Start()
    {
        EquipItem(ItemID.Fireball01);
        //EquipItem(ItemID.IceSpear01);

    }

    public void SetBossSheet(BossSheet bossSheet) {
        if (IsPlayer) {
            Debug.LogError("This method should only be called for NPCs");
            return;
        }

        CharacterSheet = bossSheet;
    }

    private void Update()
    {
        ChargeAttackLeft();
        ChargeAttackRight();
    }

    public void Move(Vector3 direction)
    {
        Move(new Vector2(direction.x, direction.z));
    }

    public void Move(Vector2 direction)
    {
        if (isLocked) return;
        int attackPenalty = 0 + (isChargingLeft ? 1 : 0) + (isChargingRight ? 1 : 0);

        lastMovingDirection = new Vector3(direction.x, 0, direction.y);
        float effectiveMoveSpeed = moveSpeed * (IsPlayer ? CheatsManager.PlayerSpeedModifier : 1);
        characterController.Move(lastMovingDirection * effectiveMoveSpeed * Time.deltaTime * movementPenalties[attackPenalty]);
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
        if (!CharacterSheet.Equipment.ContainsKey(ItemSlot.LeftArm) || CharacterSheet.Equipment[ItemSlot.LeftArm] == null) return;
        isChargingLeft = true;
        chargingLeft = 0;
        leftWeapon.LaunchAttack(chargingLeft, WeapoonChargeState.StartCharging);
    }

    private void ChargeAttackLeft()
    {
        if (!isChargingLeft) return;
        chargingLeft += Time.deltaTime;
        leftWeapon.LaunchAttack(chargingLeft, WeapoonChargeState.Charging);
    }

    public void LaunchAttackLeft()
    {
        if (!isChargingLeft) return;
        isChargingLeft = false;
        leftWeapon.LaunchAttack(chargingLeft, WeapoonChargeState.Release);
    }

    public void StartChargeAttackRight()
    {
        if (!CharacterSheet.Equipment.ContainsKey(ItemSlot.RightArm) || CharacterSheet.Equipment[ItemSlot.RightArm] == null) return;

        isChargingRight = true;
        chargingRight = 0;
        rightWeapon.LaunchAttack(chargingRight, WeapoonChargeState.StartCharging);
    }

    private void ChargeAttackRight()
    {
        if (!isChargingRight) return;
        chargingRight += Time.deltaTime;
        rightWeapon.LaunchAttack(chargingRight, WeapoonChargeState.Charging);
    }

    public void LaunchAttackRight()
    {
        if (!isChargingRight) return;
        isChargingRight = false;
        rightWeapon.LaunchAttack(chargingRight, WeapoonChargeState.Release);
    }

    [Button]
    public void EquipItem(ItemID itemID) 
    {
        CharacterSheet.Equip(ItemID.Fireball01);
    }
}
