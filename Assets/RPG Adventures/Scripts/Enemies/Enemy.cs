using UnityEngine;
using UnityEngine.AI;


namespace RPG {
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float detectionRange = 10f;
        [SerializeField] float detectionAngle = 90f;

        Character player;
        NavMeshAgent navMeshAgent;
        // Start is called before the first frame update
        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            player = DetectPlayer();
            if (player == null) return;

            Vector3 targetPos = player.transform.position;
            navMeshAgent.SetDestination(targetPos);
        }

        Character DetectPlayer()
        {
            if (Character.Instance == null) return null;

            Vector3 distanceToPlayer = Character.Instance.transform.position - transform.position;
            distanceToPlayer.y = 0f;

            if (distanceToPlayer.magnitude <= detectionRange)
            {
                if(Vector3.Dot(distanceToPlayer.normalized,transform.forward)>
                    Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
                {
                    return Character.Instance;
                }
            }

            return null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color color = new Color(0.45f, 0, 0.25f, 0.5f);
            UnityEditor.Handles.color = color;

            Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(transform.position,
                                             Vector3.up,
                                             rotatedForward,
                                             detectionAngle, detectionRange);
        }
#endif
    }
}

