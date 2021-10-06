using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{


    NavMeshAgent navMeshAgent;
    Animator anim;

    public bool NavMeshIsStopped
    {
        get
        {
            return navMeshAgent.isStopped;
        }
        set
        {
            navMeshAgent.isStopped = value;
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnAnimatorMove()
    {
        if (navMeshAgent == null) return;
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            navMeshAgent.speed = (anim.deltaPosition / Time.fixedDeltaTime).magnitude;
        Debug.Log((anim.deltaPosition/Time.fixedDeltaTime).magnitude);
    }

    public void SetDestination(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }
}
