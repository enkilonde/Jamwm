using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction : MonoBehaviour
{
    public CustomCharacterController characterController;
    public ItemSlot slot;

    public ArmBehaviour armBehaviour;

    

    delegate void attackMethod(Item item, float charge, WeapoonChargeState state);

    public void LaunchAttack(float charge, WeapoonChargeState state)
    {
        Weapon weapon = (Weapon)((CharacterSheet)characterController.CharacterSheet).Equipment[slot];

        switch (state)
        {
            case WeapoonChargeState.StartCharging:
                weapon.StartCharging(characterController);
                break;
            case WeapoonChargeState.Charging:
                weapon.Charging(charge);
                break;
            case WeapoonChargeState.Release:
                armBehaviour.animator.SetTrigger("LaunchAttack");

                weapon.Attack(charge, armBehaviour.hand.position);

                break;
            default:
                break;
        }
    

        GetMethod(weapon).Invoke(weapon, charge, state);

    }

    private void LateUpdate()
    {
        if (characterController.CharacterSheet == null || !characterController.CharacterSheet.Equipment.ContainsKey(slot)) return;
        Item item = characterController.CharacterSheet.Equipment[slot];
        transform.position = armBehaviour.hand.position;
        transform.rotation = armBehaviour.hand.rotation;
    }

    private attackMethod GetMethod(Item item)
    {
        switch (item.ID)
        {
            case ItemID.Fireball01: return Fireball;
            case ItemID.IceSpear01: return IceSpear;
            case ItemID.Sword01: return Sword;
            case ItemID.Axe01: return Axe;
            case ItemID.Shield01: return Shield;

            default:
                throw new System.Exception("Why are you trying to activate the item '" + item.Name + "' in the slot '" + slot + "', you should not be able to do it, or it need to be implemented");
        }
    }

    private void Fireball(Item item, float charge, WeapoonChargeState state)
    {
        if(state == WeapoonChargeState.StartCharging)
            armBehaviour.animator.SetTrigger("Cast");
    }

    private void IceSpear(Item item, float charge, WeapoonChargeState state)
    {
        if (state == WeapoonChargeState.StartCharging)
            armBehaviour.animator.SetTrigger("Cast");
    }

    private void Sword(Item item, float charge, WeapoonChargeState state)
    {
        if (state == WeapoonChargeState.StartCharging)
            armBehaviour.animator.SetTrigger("Attack");
    }

    private void Axe(Item item, float charge, WeapoonChargeState state)
    {
        if (state == WeapoonChargeState.StartCharging)
            armBehaviour.animator.SetTrigger("Attack");
    }

    private void Shield(Item item, float charge, WeapoonChargeState state)
    {
        if (state == WeapoonChargeState.StartCharging)
            armBehaviour.animator.SetTrigger("Attack");
    }



}
