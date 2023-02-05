using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomIAInputsMove : MonoBehaviour
{
    public CustomCharacterController player;
    public CustomCharacterController IA;
    public CustomIAInputs iaInputs;


    [ReadOnly]
    public IAState state;


    public Animator animatorIA;


    [Range(0, 1)]
    public float rushChance;
    public float minRushDistance;
    public float minDashDistance;
    public float wantedRushDistance;
    public float minStraffDistance;
    public float dashChance;

    [Range(0, 1)]
    public float moveChance;

    // Update is called once per frame
    void Update()
    {
        if (IA.CharacterSheet == null) return;
        if (!iaInputs.enabled) return;

        AnimatorStateInfo animatorState = animatorIA.GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsName("Choose"))
        {
            ChooseState();
        }

        switch (state)
        {
            case IAState.Idle:
                break;
            case IAState.strafeLeft:
                UpdateStrafeLeft();
                break;
            case IAState.strafeRight:
                UpdateStrafeRight();
                break;
            case IAState.Rush:
                UpdateRush();
                break;
            case IAState.Dodge:
                break;
            default:
                break;
        }

        UpdateTurning();
    }



    private void ChooseState()
    {
        if (DoRushPlayer()) return;
        if (DoMovement()) return;
        DoIdle();

    }

    private void UpdateTurning()
    {
        Vector3 direction = player.transform.position - transform.position;
        IA.Turn(new Vector2(direction.x, direction.z));
    }

    #region StartStates


 



    private bool DoRushPlayer()
    {
        if (!CanRush()) return false;
        if (Random.value > rushChance) return false;
        animatorIA.SetTrigger("_rush");

        return true;
    }

    private bool DoMovement()
    {
        if (!CanStraff()) return false;
        if (Random.value > moveChance) return false;
        animatorIA.SetTrigger("_move");

        return true;
    }

    private bool DoIdle()
    {
        animatorIA.SetTrigger("_idle");
        return true;
    }



    private bool CanStraff()
    {
        return Vector3.Distance(transform.position, player.transform.position) > minStraffDistance;
    }

    private bool CanRush()
    {
        return Vector3.Distance(transform.position, player.transform.position) > minRushDistance;
    }
    #endregion


    #region StatesUpdates

    public void UpdateStrafeLeft()
    {
        Vector3 direction = Vector3.Cross((player.transform.position - transform.position).normalized, transform.up);
        IA.Move(direction);
    }

    public void UpdateStrafeRight()
    {
        Vector3 direction = -Vector3.Cross((player.transform.position - transform.position).normalized, transform.up);
        IA.Move(direction);
    }

    private void UpdateRush()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        IA.Move(direction);
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= wantedRushDistance)
            animatorIA.SetTrigger("choose");
        else if (distance > minDashDistance && Random.value > dashChance)
            IA.Dash();
    }

    #endregion

}
