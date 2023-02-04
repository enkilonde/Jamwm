using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    public float speed;

    public AnimationCurve scaleByCharge;

    private float damage;
    public void Setup(float charge, int _damage)
    {
        transform.localScale = Vector3.one * scaleByCharge.Evaluate(charge);
        damage = _damage * charge;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        CustomCharacterController character = other.GetComponent<CustomCharacterController>();
        if(character != null)
        {
            character.CharacterSheet.Hit(Mathf.CeilToInt(damage));
        }
    }
}
