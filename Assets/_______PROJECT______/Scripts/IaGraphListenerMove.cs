using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaGraphListenerMove : MonoBehaviour
{
    public Animator iaAnimator;
    public CustomIAInputsMove iaInputs;



    public void Idle()
    {
        iaInputs.state = IAState.Idle;
    }

    public void Rush()
    {
        iaInputs.state = IAState.Rush;
    }

    public void Dash()
    {

    }

    public void Choose()
    {
        iaInputs.state = IAState.Choosing;
    }

    public void StrafeLeft()
    {
        iaInputs.state = IAState.strafeLeft;

    }

    public void StrafeRight()
    {
        iaInputs.state = IAState.strafeRight;

    }
}
