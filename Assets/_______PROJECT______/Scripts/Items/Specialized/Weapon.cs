using System;
using System.Collections;
using UnityEngine;

public enum DamageType {

    Physical,
    Magical

}

public class Weapon : Item {

    public override ItemKind Kind => ItemKind.Weapon;

    [Header("Weapon-only stats")]
    public DamageType DamageType;

    public void StartCharging()
    {
        chargeFX.Play();
    }

    public void Attack(float charge, CustomCharacterController owner, Vector3 handPosition)
    {
        chargeFX.Stop();
        SendProjectile(charge, owner, handPosition);
    }

    internal void SendProjectile(float charge, CustomCharacterController owner, Vector3 spawnPosition)
    {
        if (projectilePrefab == null) return;
        if (projectileDelay <= 0) SpawnProjectile(charge, owner, spawnPosition);
        else StartCoroutine(WaitSendProjectile(charge, owner, spawnPosition));
    }

    private void SpawnProjectile(float charge, CustomCharacterController owner, Vector3 spawnPosition)
    {
        ProjectileBehaviour proj = Instantiate(projectilePrefab, spawnPosition, owner.transform.rotation);

        float rawDamages;
        switch (DamageType) {
            case DamageType.Physical:
                rawDamages = owner.CharacterSheet.Stats[PlayerStats.Strength];
                break;
            case DamageType.Magical:
                rawDamages = owner.CharacterSheet.Stats[PlayerStats.MagicPower];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (charge < 1) {
            charge = 1;
        } else if (charge > 3) {
            charge = 3;
        }
        rawDamages *= charge;

        proj.Setup(charge, rawDamages);
    }

    private IEnumerator WaitSendProjectile(float charge, CustomCharacterController owner, Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(projectileDelay);
        SpawnProjectile(charge, owner, spawnPosition);
    }

}