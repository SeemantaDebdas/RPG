using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [System.Serializable]
    public class AttackPoints
    {
        public Transform rootPosition;
        public float radius;
        public Vector3 offset;
    }

    public AttackPoints[] attackPoints = new AttackPoints[0];

    public int weaponDamage;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach(AttackPoints attackPoint in attackPoints)
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
