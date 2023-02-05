using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IAState
{
    Choosing,
    Idle,
    strafeLeft,
    strafeRight,
    Rush,
    Attack,
    Dodge,
}
public class CustomIAInputs : MonoBehaviour
{
    public CustomCharacterController player;
    public CustomCharacterController IA;


    [ReadOnly]
    public IAState state;


    public Animator animatorIA;

    [Range(0, 1)]
    public float attackChance;


    [Range(0, 1)]
    public float moveChance;

    // Update is called once per frame
    void Update()
    {
        if (IA.CharacterSheet == null) return;

        AnimatorStateInfo animatorState = animatorIA.GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsName("Choose"))
        {
            ChooseState();
        }

        switch (state)
        {
            case IAState.Idle:
                break;
            case IAState.Attack:
                break;
            case IAState.Dodge:
                break;
            default:
                break;
        }

    }



    private void ChooseState()
    {
        if (DoAttack()) return;

        DoIdle();

    }


    #region StartStates


    private bool DoAttack()
    {
        if (!CanAttack()) return false;
        if (Random.value > attackChance) return false;
        //attack
        animatorIA.SetTrigger("_attack");
        return true;
    }



    private bool DoIdle()
    {
        animatorIA.SetTrigger("_idle");
        return true;
    }


    private bool CanAttack()
    {
        return true;
    }

#endregion

}
