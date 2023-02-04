using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction : MonoBehaviour
{
    public CustomCharacterController characterController;
    public ItemSlot slot;

    delegate void attackMethod(Item item, float charge);

    public void LaunchAttack(float charge)
    {
        Item item = characterController.PlayerSheet.Equipment[slot];

        GetMethod(item).Invoke(item, charge);

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

    private void Fireball(Item item, float charge)
    {
        //instantiate fireball
    }

    private void IceSpear(Item item, float charge)
    {

    }

    private void Sword(Item item, float charge)
    {
        //launch anim
    }

    private void Axe(Item item, float charge)
    {

    }

    private void Shield(Item item, float charge)
    {

    }



}
