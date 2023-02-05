using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {
    CustomCharacterController owner;
    private const int PoolingDistance = 60;

    public float speed;
    public float maxLifetime;

    private float lifetime;

    public AnimationCurve scaleByCharge;

    public Transform fullCharge;
    public Transform normalCharge;

    private float damage;
    public void Setup(float charge, float rawDamages, bool isFull, CustomCharacterController owner)
    {
        transform.localScale = Vector3.one * scaleByCharge.Evaluate(charge);
        damage = rawDamages;
        this.owner= owner;

        if(isFull && fullCharge!= null )
        {
            fullCharge.gameObject.SetActive(true);
            normalCharge.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() {
        Transform t = transform;
        float deltaTime = Time.deltaTime;

        var newPos = t.position;
        newPos += t.forward * speed * deltaTime;
        t.position = newPos;

        lifetime += deltaTime;
        if (maxLifetime > 0 && lifetime >= maxLifetime) {
            Destroy(this.gameObject);
            return;
        }

        if (newPos.x is < -PoolingDistance or > PoolingDistance || 
            newPos.z is < -PoolingDistance or > PoolingDistance
        ) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null) return;
        if (other.gameObject == owner.gameObject) return;

        CustomCharacterController character = other.GetComponent<CustomCharacterController>();
        if(character != null) {
            float defense = character.CharacterSheet.Stats[PlayerStats.Defense];
            float reduction = Mathf.Max(defense * 0.5f, damage * 0.5f);
            int lostHP = Mathf.CeilToInt(reduction);
            character.CharacterSheet.Hit(lostHP);
            Destroy(this.gameObject);
        }
    }
}
