using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMBase : StateMachineBehaviour
{
    //state ai

    [SerializeField]
    protected GameObject NPC, target;
    [SerializeField]
    protected float runSpeed = 10f,
        rotationSpeed = 2f,
        accuracy = 5f,
        acceleration = 20f;
    protected NavMeshAgent agent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        NPC = animator.gameObject;
        agent = NPC.GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player").gameObject;
    }
}
