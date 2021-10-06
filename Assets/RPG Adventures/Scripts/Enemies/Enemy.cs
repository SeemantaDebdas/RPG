using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace RPG {
    public class Enemy : MonoBehaviour
    {

        [SerializeField] float timeToStopPursuit = 2f;
        [SerializeField] float pursuitResetTimer = 2f;
        [SerializeField] float attackingDistance = 1.1f;

        public PlayerScanner playerScanner;
        Character player;
        Animator anim;
        EnemyController enemyController;

        Vector3 originPosition;
        float timeSinceLostTarget;

        //animator
        readonly int StopBool = Animator.StringToHash("StopBool");
        readonly int ReturnBool = Animator.StringToHash("ReturnBool");
        readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");

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
            Character target = playerScanner.Detect(transform);
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
                //following player
                Vector3 distanceToPlayer = player.transform.position - transform.position;
                distanceToPlayer.y = 0;
                if (distanceToPlayer.magnitude < attackingDistance)
                {
                    Debug.Log("Attacking");
                    enemyController.StopFollowTarget();
                    anim.SetTrigger(AttackTrigger);
                }
                else
                {
                    enemyController.SetDestination(player.transform.position);
                    anim.SetBool(ReturnBool, false);
                }
                
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

        

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color color = new Color(0.45f, 0, 0.25f, 0.5f);
            UnityEditor.Handles.color = color;

            Vector3 rotatedForward = Quaternion.Euler(0, -playerScanner.detectionAngle * 0.5f, 0) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(transform.position,
                                             Vector3.up,
                                             rotatedForward,
                                             playerScanner.detectionAngle, playerScanner.detectionRange);
        }
#endif
    }
}

