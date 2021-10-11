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
            public Transform rootTransform;
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
                        ap.rootTransform.position + ap.rootTransform.TransformVector(ap.offset);
                    Vector3 attackVector = worldPos - originAttackPos[i];

                    Debug.Log("WorldPosition: " + worldPos);
                    Debug.Log("Origin Attack Position: " + originAttackPos[i]);
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
                    ap.rootTransform.position + ap.rootTransform.TransformVector(ap.offset);
            }

        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach (AttackPoints attackPoint in attackPoints)
            {
                if (attackPoint.rootTransform != null)
                {
                    Vector3 worldOffset = attackPoint.rootTransform.TransformVector(attackPoint.offset);
                    Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    Gizmos.DrawSphere(attackPoint.rootTransform.position + worldOffset, attackPoint.radius);
                }
            }
        }
#endif
    }
}
