using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : FSMBase
{
    //state: enemy attacks player

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        NPC.transform.LookAt(target.transform);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //stops enemy
        NPC.GetComponent<Rigidbody>().velocity = Vector3.zero;
        agent.velocity = Vector3.zero;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
