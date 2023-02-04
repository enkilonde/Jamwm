using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchRandomTrigger : StateMachineBehaviour
{
    public Vector2 waitTime;
    private float waitTimer;

    public List<string> triggers;
    public List<int> weights;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTimer = Random.Range(waitTime.x, waitTime.y);
        if(waitTimer <= 0)
            LaunchTrigger(animator);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
            LaunchTrigger(animator);
    }

    private void LaunchTrigger(Animator animator)
    {
        animator.SetTrigger(PickRandomElement());
        waitTimer = Random.Range(waitTime.x, waitTime.y) + 9999;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}


    string PickRandomElement()
    {
        if(weights.Count < triggers.Count) 
            return triggers[Random.Range(0, triggers.Count)];

        int totalWeight = 0;

        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        int randomIndex = Random.Range(0, totalWeight);

        for (int i = 0; i < triggers.Count; i++)
        {
            if (randomIndex < weights[i])
            {
                return triggers[i];
            }

            randomIndex -= weights[i];
        }

        return triggers[0];
    }

}
