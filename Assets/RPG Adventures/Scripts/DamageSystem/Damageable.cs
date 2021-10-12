using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public partial class Damageable : MonoBehaviour
    {
        [SerializeField] int maxHitPoints;
        [Range(0, 360f)]
        public float hitAngle;

        public void Damage(DamageMessage data)
        {
            Debug.Log(data.amount);
            Debug.Log(data.damageSource);
            Debug.Log(data.damager);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0, 0.5f, 0.7f, 0.6f);

            Vector3 rotatedForward = Quaternion.AngleAxis(-hitAngle * 0.5f, Vector3.up) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(
                                             transform.position,
                                             transform.up,
                                             rotatedForward,
                                             hitAngle,
                                             1.0f);
        }
#endif
    }

}
