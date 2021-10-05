using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnAnimatorMove()
    {
        if (navMeshAgent == null) return;
        if(!Mathf.Approximately((anim.deltaPosition / Time.fixedDeltaTime).magnitude,0f))
            navMeshAgent.speed = (anim.deltaPosition / Time.fixedDeltaTime).magnitude;
        Debug.Log((anim.deltaPosition/Time.fixedDeltaTime).magnitude);
    }
}
