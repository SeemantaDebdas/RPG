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
            return (navMeshAgent.enabled) ? navMeshAgent.isStopped : true;
        }
        set
        {
            navMeshAgent.isStopped = value;
        }
    }

    public bool NavMeshActive
    {
        get { return navMeshAgent.enabled; }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnAnimatorMove()
    {
        if (navMeshAgent == null) return;
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && navMeshAgent.enabled)
            navMeshAgent.speed = (anim.deltaPosition / Time.fixedDeltaTime).magnitude;
    }

    public void SetDestination(Vector3 position)
    {
        if (!navMeshAgent.enabled) navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(position);
    }

    public void StopFollowTarget()
    {
        navMeshAgent.enabled = false;
    }
}
