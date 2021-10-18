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
        RaycastHit[] rayCastHitCache = new RaycastHit[32];

        public LayerMask targetLayers;
        public bool isAttacking = false;
        public int weaponDamage;
        GameObject weaponOwner;

        [SerializeField] RandomAudioPlayer swingSound;
        [SerializeField] RandomAudioPlayer impactSound;

        private void FixedUpdate()
        {
            if (isAttacking)
            {
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoints ap = attackPoints[i];
                    Vector3 worldPos = 
                        ap.rootTransform.position + ap.rootTransform.TransformVector(ap.offset);
                    Vector3 attackVector = (worldPos - originAttackPos[i]).normalized;
                    Ray ray = new Ray(worldPos, attackVector);
                    Debug.DrawRay(worldPos, attackVector, Color.red, 1.0f);

                    //checking for the number of contacts the attack vector made
                    int contactNumber = Physics.SphereCastNonAlloc(
                                            ray,
                                            ap.radius,
                                            rayCastHitCache,
                                            attackVector.magnitude,
                                            ~0,
                                            QueryTriggerInteraction.Ignore);

                    for(int contact = 0; contact < contactNumber; contact++)
                    {
                        Collider collider = rayCastHitCache[contact].collider;
                        if (collider != null)
                        {
                            CheckDamage(collider, ap);
                        }
                    }

                    originAttackPos[0] = worldPos;
                }
            }
        }

        public void SetOwner(GameObject weaponOwner)
        {
            this.weaponOwner = weaponOwner;
        }

        public void SetTargetLayer(LayerMask targetLayer)
        {
            this.targetLayers = targetLayer;
        }

        void CheckDamage(Collider other, AttackPoints ap)
        {
            if ((targetLayers.value & 1<<other.gameObject.layer) == 0) return;

            Damageable damagable = other.GetComponent<Damageable>();
            if (damagable != null)
            {
                Damageable.DamageMessage data;
                data.amount = weaponDamage;
                data.damager = this;
                data.damageSource = weaponOwner;
                damagable.Damage(data);

                impactSound.PlayRandomClip();
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
                    ap.rootTransform.position + ap.rootTransform.TransformDirection(ap.offset);
            }

            swingSound.PlayRandomClip();

        }

        public void StopAttack()
        {
            isAttacking = false;
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
