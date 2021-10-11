using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace RPG {
    public class Enemy : MonoBehaviour,IAttackAnimListener
    {

        [SerializeField] float timeToStopPursuit = 2f;
        [SerializeField] float pursuitResetTimer = 2f;
        [SerializeField] float attackingDistance = 1.1f;

        public PlayerScanner playerScanner;
        Character followTarget;
        Animator anim;
        EnemyController enemyController;

        Vector3 originPosition;
        Quaternion originalRotation;
        float timeSinceLostTarget;
        Vector3 distanceToOrigin;

        //animator
        readonly int StopBool = Animator.StringToHash("StopBool");
        readonly int ReturnBool = Animator.StringToHash("ReturnBool");
        readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");

        // Start is called before the first frame update
        void Awake()
        {
            anim = GetComponent<Animator>();

            anim.SetBool(StopBool, true);

            enemyController = GetComponent<EnemyController>();
            originPosition = transform.position;
            originalRotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {

            var target = HandleTarget(); 
            var isNearBase = IsNearBase();

            if (isNearBase)
            {
                ResetRotation();
            }
            
            if(target == null && isNearBase)
            {
                anim.SetBool(StopBool, true);
            }
        }

        private Character HandleTarget()
        {
            Character detectedTarget = playerScanner.Detect(transform);
            if (detectedTarget != null) followTarget = detectedTarget;

            if (followTarget != null)
            {
                AttackOrFollow();
                if (detectedTarget == null)
                    StopPursuit();
                else
                    timeSinceLostTarget = 0;
            }

            return followTarget;
        }

        private bool IsNearBase()
        {
            distanceToOrigin = originPosition - transform.position;
            distanceToOrigin.y = 0;
            return distanceToOrigin.magnitude < 0.1f;
        }

        private void StopPursuit()
        {
            timeSinceLostTarget += Time.deltaTime;

            if (timeSinceLostTarget >= timeToStopPursuit)
            {
                followTarget = null;
                anim.SetBool(StopBool, true);
                StartCoroutine(WaitBeforeReturn());
            }
        }

        private void AttackOrFollow()
        {
            Vector3 distanceToPlayer = followTarget.transform.position - transform.position;
            distanceToPlayer.y = 0;
            if (distanceToPlayer.magnitude < attackingDistance)
            {
                AttackTarget(distanceToPlayer);
            }
            else
            {
                anim.SetBool(StopBool, false);
                FollowTarget();
            }
        }

        void AttackTarget(Vector3 distanceToPlayer)
        {
            Quaternion targetRotation = Quaternion.LookRotation(distanceToPlayer);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                          targetRotation,
                                                          360 * Time.deltaTime);
            enemyController.StopFollowTarget();
            anim.SetTrigger(AttackTrigger);
        }

        void FollowTarget()
        {
            enemyController.SetDestination(followTarget.transform.position);
            anim.SetBool(ReturnBool, false);
        }

        private void ResetRotation()
        {
            if (IsNearBase())
            {
                Quaternion targetRotation = Quaternion.RotateTowards(
                                                                    transform.rotation,
                                                                    originalRotation,
                                                                    400 * Time.fixedDeltaTime);
                transform.rotation = targetRotation;
            }
        }

        IEnumerator WaitBeforeReturn()
        {
            yield return new WaitForSeconds(pursuitResetTimer);
            enemyController.SetDestination(originPosition);
            anim.SetBool(ReturnBool, true);
            anim.SetBool(StopBool, false);
        }

        public void MeleeAttackStart(){}

        public void MeleeAttackStop(){}

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
            UnityEditor.Handles.DrawSolidArc(transform.position,
                                             Vector3.up,
                                             rotatedForward,
                                             360, playerScanner.meleeAttackRange);
        }


#endif
    }
}

