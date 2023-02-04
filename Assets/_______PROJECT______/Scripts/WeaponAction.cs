using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeapoonChargeState
{
    StartCharging,
    Charging, 
    Release
}

public class WeaponAction : MonoBehaviour
{
    public CustomCharacterController characterController;
    public ItemSlot slot;
    public ArmBehaviour armBehaviour;

    delegate void attackMethod(Item item, float charge, WeapoonChargeState state);

    private void LateUpdate()
    {
        transform.position = armBehaviour.hand.position;
    }

    public void LaunchAttack(float charge, WeapoonChargeState state)
    {
        Item item = characterController.PlayerSheet.Equipment[slot];

        GetMethod(item).Invoke(item, charge, state);
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
        //instantiate fireball
        switch (state)
        {
            case WeapoonChargeState.StartCharging:
                armBehaviour.animator.SetTrigger("StartChargingSpell");
                break;
            case WeapoonChargeState.Charging:
                armBehaviour.animator.SetFloat("ChargeAmount", charge);
                break;
            case WeapoonChargeState.Release:
                armBehaviour.animator.SetFloat("LaunchAttack", charge);
                break;
            default:
                break;
        }
    }

    private void IceSpear(Item item, float charge, WeapoonChargeState state)
    {

    }

    private void Sword(Item item, float charge, WeapoonChargeState state)
    {
        //launch anim
    }

    private void Axe(Item item, float charge, WeapoonChargeState state)
    {

    }

    private void Shield(Item item, float charge, WeapoonChargeState state)
    {

    }



}
