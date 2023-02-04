using System;
using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    public abstract ItemKind Kind { get; }

    public ParticleSystem chargeFX;

    public ProjectileBehaviour projectilePrefab;
    public float projectileDelay;

    [Header("Item Metadata")]
    public string Name;
    public ItemID ID;

    [Header("Stats Modifiers")]
    public int Strength;
    public int Magic;
    public int AttackSpeed;
    public int MovementSpeed;
    public int Defense;
    public int MaxHp;

    // Item State
    public bool Equipped { get; set; }


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
        proj.Setup(charge, Strength);
    }

    private IEnumerator WaitSendProjectile(float charge, CustomCharacterController owner, Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(projectileDelay);
        SpawnProjectile(charge, owner, spawnPosition);
    }
}