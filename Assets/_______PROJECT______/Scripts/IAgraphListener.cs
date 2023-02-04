using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAgraphListener : MonoBehaviour
{
    public Animator iaAnimator;
    public CustomIAInputs iaInputs;

    
    public void StartAttack()
    {
        iaInputs.IA.StartChargeAttackLeft();
    }

    public void LaunchAttack()
    {
        iaInputs.IA.LaunchAttackLeft();
    }

    public void Idle()
    {
        iaInputs.state = CustomIAInputs.IAState.Idle;
    }

    public void Rush()
    {
        iaInputs.state = CustomIAInputs.IAState.Rush;
    }

    public void Dash()
    {

    }

    public void Choose()
    {
        iaInputs.state = CustomIAInputs.IAState.Choosing;
    }

    public void StrafeLeft()
    {
        iaInputs.state = CustomIAInputs.IAState.strafeLeft;

    }

    public void StrafeRight()
    {
        iaInputs.state = CustomIAInputs.IAState.strafeRight;

    }
}
