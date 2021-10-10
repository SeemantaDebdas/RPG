using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Weapon : MonoBehaviour
    {
        [System.Serializable]
        public class AttackPoints
        {
            public Transform rootPosition;
            public float radius;
            public Vector3 offset;
        }

        public AttackPoints[] attackPoints = new AttackPoints[0];
        Vector3[] originAttackPos;
        public bool isAttacking = false;
        public int weaponDamage;

        private void FixedUpdate()
        {
            if (isAttacking)
            {
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoints ap = attackPoints[i];
                    Vector3 worldPos = 
                        ap.rootPosition.position + ap.rootPosition.TransformVector(ap.offset);
                    Vector3 attackVector = worldPos - originAttackPos[i];

                    Ray r = new Ray(worldPos, attackVector);
                    Debug.DrawRay(worldPos, attackVector, Color.red, 1.0f);
                }
            }
        }

        public void BeginAttack()
        {
            isAttacking = true;
            originAttackPos = new Vector3[attackPoints.Length];

            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoints ap = attackPoints[i];
                originAttackPos[i] = 
                    ap.rootPosition.position + ap.rootPosition.TransformDirection(ap.offset);
            }

        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach (AttackPoints attackPoint in attackPoints)
            {
                if (attackPoint.rootPosition != null)
                {
                    Vector3 localOffset = attackPoint.rootPosition.TransformVector(attackPoint.offset);
                    Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    Gizmos.DrawSphere(attackPoint.rootPosition.position + localOffset, attackPoint.radius);
                }
            }
        }
#endif
    }
}
