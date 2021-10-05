using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace RPG {
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float detectionRange = 10f;
        [SerializeField] float detectionAngle = 90f;
        [SerializeField] float timeToStopPursuit = 2f;
        [SerializeField] float pursuitResetTimer = 2f;

        Character player;
        Animator anim;
        EnemyController enemyController;

        Vector3 originPosition;
        float timeSinceLostTarget;

        //animator
        readonly int StopBool = Animator.StringToHash("StopBool");
        readonly int ReturnBool = Animator.StringToHash("ReturnBool");

        // Start is called before the first frame update
        void Awake()
        {
            anim = GetComponent<Animator>();
            enemyController = GetComponent<EnemyController>();
            originPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Character target = DetectPlayer();
            HandleAnimaton();

            if (player == null)
            {
                if (target != null)
                {
                    //if player is null but target detected
                    //basically when player enters enemy detection range
                    player = target;
                }
            }
            else
            {
                enemyController.SetDestination(player.transform.position);
                anim.SetBool(ReturnBool, false);
                if (target == null)
                {
                    //if player goes out of range and we want
                    //to stop our enemy from chasing
                    timeSinceLostTarget += Time.deltaTime;

                    if (timeSinceLostTarget >= timeToStopPursuit)
                    {
                        player = null;
                        enemyController.NavMeshIsStopped = true;
                        StartCoroutine(WaitOnPursuit());
                    }

                }
                else
                {
                    timeSinceLostTarget = 0;
                }
            }
        }

        void HandleAnimaton()
        {
            Vector3 distanceToOrigin = originPosition - transform.position;
            distanceToOrigin.y = 0;
            anim.SetBool(StopBool, enemyController.NavMeshIsStopped || distanceToOrigin.magnitude < 0.1f);
        }

        IEnumerator WaitOnPursuit()
        {
            yield return new WaitForSeconds(pursuitResetTimer);
            enemyController.NavMeshIsStopped = false;
            enemyController.SetDestination(originPosition);
            anim.SetBool(ReturnBool, true);
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

