using Sirenix.OdinInspector;
using Sirenix.Utilities;
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
    private float chargingLeft;
    private float chargingRight;

    private float wantedAimAlpha = 0;
    public float currentAimAlpha = 0;
    public LineRenderer aimVisual;
    public Vector2 aimVisualFadeInOutSpeed;

    private float _latestLeftAttackTime;
    private float _latestRightAttackTime;

    private bool isLocked => isDashing; //may add more conditions

    private void Awake() {
        if (IsPlayer) {
            CharacterSheet = new PlayerSheet(playerVisual, this.transform);
        }
        dashTimer = dashCooldown;

    }

    private void Start()
    {
        if(IsPlayer)
        {
            EquipItem(ItemID.Fireball01);
            //EquipItem(ItemID.IceSpear01);        
            EquipItem(ItemID.Sword01);
        }

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
        SetAimAlpha();
    }

    private void SetAimAlpha()
    {
        if(currentAimAlpha < wantedAimAlpha)
        {
            currentAimAlpha = Mathf.Lerp(currentAimAlpha, wantedAimAlpha, Time.deltaTime * aimVisualFadeInOutSpeed.x);
        }
        else
        {
            currentAimAlpha = Mathf.Lerp(currentAimAlpha, wantedAimAlpha, Time.deltaTime * aimVisualFadeInOutSpeed.y);
        }

        Color col = aimVisual.material.color;
        col.a = currentAimAlpha;
        aimVisual.material.color = col;
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

        float speedStat = CharacterSheet.Stats[PlayerStats.MovementSpeed];
        float speedFactor = speedStat / 100f;
        float effectiveMoveSpeed = moveSpeed * speedFactor * (IsPlayer ? CheatsManager.PlayerSpeedModifier : 1);

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

#region Left-Hand Attack

    public void StartChargeAttackLeft()
    {
        if (!CharacterSheet.Equipment.ContainsKey(ItemSlot.LeftArm) || CharacterSheet.Equipment[ItemSlot.LeftArm] == null) return;

        // Attack Speed factoring in this "anti-spam" mechanism
        if (Time.realtimeSinceStartup - _latestLeftAttackTime < GetMinDelayBetweenAttack()) return;

        isChargingLeft = true;
        chargingLeft = 0;
        leftWeapon.LaunchAttack(chargingLeft, WeapoonChargeState.StartCharging);
        wantedAimAlpha++;
    }

    private void ChargeAttackLeft()
    {
        if (!isChargingLeft) return;
        chargingLeft += Time.deltaTime;
        leftWeapon.LaunchAttack(chargingLeft, WeapoonChargeState.Charging);
        _latestLeftAttackTime = Time.realtimeSinceStartup;
    }

    public void LaunchAttackLeft()
    {
        if (!isChargingLeft) return;
        isChargingLeft = false;
        leftWeapon.LaunchAttack(chargingLeft, WeapoonChargeState.Release);
        _latestLeftAttackTime = Time.realtimeSinceStartup;
        wantedAimAlpha--;
    }

#endregion

#region Right-Hand Attack
    
    public void StartChargeAttackRight()
    {
        if (!CharacterSheet.Equipment.ContainsKey(ItemSlot.RightArm) || CharacterSheet.Equipment[ItemSlot.RightArm] == null) return;
        
        // Attack Speed factoring in this "anti-spam" mechanism
        if (Time.realtimeSinceStartup - _latestRightAttackTime < GetMinDelayBetweenAttack()) return;

        isChargingRight = true;
        chargingRight = 0;
        rightWeapon.LaunchAttack(chargingRight, WeapoonChargeState.StartCharging);
        wantedAimAlpha++;
    }

    private void ChargeAttackRight()
    {
        if (!isChargingRight) return;
        chargingRight += Time.deltaTime;
        rightWeapon.LaunchAttack(chargingRight, WeapoonChargeState.Charging);
        _latestRightAttackTime = Time.realtimeSinceStartup;
    }

    public void LaunchAttackRight()
    {
        if (!isChargingRight) return;
        isChargingRight = false;
        rightWeapon.LaunchAttack(chargingRight, WeapoonChargeState.Release);
        _latestRightAttackTime = Time.realtimeSinceStartup;
        wantedAimAlpha--;
    }
    
#endregion

    private float GetMinDelayBetweenAttack() {
        float attackSpeed = CharacterSheet.Stats[PlayerStats.AttackSpeed];
        return 100f / attackSpeed;
    }

    [Button]
    public void EquipItem(ItemID itemID) 
    {
        CharacterSheet.Equip(itemID);
    }
}
