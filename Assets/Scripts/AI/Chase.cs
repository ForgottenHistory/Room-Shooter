using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : FSMBase
{
    //state: enemy runs at player

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        agent.speed = runSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.stoppingDistance = accuracy;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (NPC.GetComponent<AnimatedEnemy>().activated)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
