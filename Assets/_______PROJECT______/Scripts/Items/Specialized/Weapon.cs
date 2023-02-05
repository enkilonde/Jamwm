using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UI.GridLayoutGroup;

public enum DamageType {

    Physical,
    Magical

}

public class Weapon : Item {

    public int weaponChargeTime;
    private bool isFullCharged = false;
    private bool isAttacking = false;

    private bool hasHit = false;

    public ParticleSystem chargeFX;
    public ParticleSystem chargeFullFX;


    public ProjectileBehaviour projectilePrefab;
    public float projectileDelay;
    public bool projectilOnlyOnFullCharge;

    private CustomCharacterController owner;
    private float charge;

    public override ItemKind Kind => ItemKind.Weapon;

    [Header("Weapon-only stats")]
    public DamageType DamageType;

    public void StartCharging(CustomCharacterController _owner)
    {
        SoundManager.INSTANCE.PlaySound(SoundInfo.SoundType.AttackCharge);

        chargeFX.Play();
        isFullCharged = false;
        owner = _owner;
        this.charge = 0;
    }    
    
    public void Charging(float charge)
    {
        this.charge = charge;
        if (charge >= weaponChargeTime && !isFullCharged)
        {
            //SoundManager.INSTANCE.PlaySound(SoundInfo.SoundType.AttackChargeEnemy);

            chargeFullFX.Play();
            isFullCharged = true;
        }
    }

    public void Attack(float charge, Vector3 handPosition)
    {
        SoundManager.INSTANCE.PlaySound(SoundInfo.SoundType.RangeAttack);

        this.charge = charge;
        chargeFX.Stop();
        SendProjectile(handPosition);
        if (projectilOnlyOnFullCharge)
        {
            isAttacking = true;
            hasHit = false;
        }
    }

    internal void SendProjectile(Vector3 spawnPosition)
    {
        if (projectilePrefab == null) return;
        if (projectilOnlyOnFullCharge && !isFullCharged) return;
        if (projectileDelay <= 0) SpawnProjectile(spawnPosition);
        else StartCoroutine(WaitSendProjectile(spawnPosition));
    }

    private void SpawnProjectile(Vector3 spawnPosition)
    {
        ProjectileBehaviour proj = Instantiate(projectilePrefab, spawnPosition, owner.transform.rotation);



        proj.Setup(charge, RawDamage(), isFullCharged, owner);
    }

    private IEnumerator WaitSendProjectile(Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(projectileDelay);
        SpawnProjectile(spawnPosition);
        isAttacking = false;
    }

    private float RawDamage()
    {
        float rawDamages;
        switch (DamageType)
        {
            case DamageType.Physical:
                rawDamages = owner.CharacterSheet.Stats[PlayerStats.Strength];
                break;
            case DamageType.Magical:
                rawDamages = owner.CharacterSheet.Stats[PlayerStats.MagicPower];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (charge < 1)
        {
            charge = 1;
        }
        else if (charge > weaponChargeTime)
        {
            charge = weaponChargeTime;
        }
        rawDamages *= charge;

        return rawDamages;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;
        if (hasHit) return;
        if (owner == null) return;
        if (other.gameObject == owner.gameObject) return;

        CustomCharacterController character = other.GetComponent<CustomCharacterController>();
        if (character != null)
        {
            float defense = character.CharacterSheet.Stats[PlayerStats.Defense];
            float reduction = Mathf.Max(defense * 0.5f, RawDamage() * 0.5f);
            int lostHP = Mathf.CeilToInt(reduction);
            character.CharacterSheet.Hit(lostHP);
            hasHit = true;
        }
    }

}