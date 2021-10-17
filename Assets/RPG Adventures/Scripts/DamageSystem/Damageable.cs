using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public partial class Damageable : MonoBehaviour
    {
        [SerializeField] int maxHitPoints;
        int currentHitPoints;
        [Range(0, 360f)]
        public float hitAngle;
        public List<MonoBehaviour> onDamageMessageReceivers;

        [SerializeField] bool isInvulnerable;
        [SerializeField] float invulnerabilityTime = 0.5f;
        float timeSinceLastHit;


        private void Awake()
        {
            currentHitPoints = maxHitPoints;
        }

        private void Update()
        {
            if (isInvulnerable)
            {
                timeSinceLastHit += Time.deltaTime;
                if (timeSinceLastHit >= invulnerabilityTime)
                {
                    isInvulnerable = false;
                    timeSinceLastHit = 0;
                }
            }
        }

        public void Damage(DamageMessage data)
        {
            if (currentHitPoints <= 0 || isInvulnerable)
                return;

            Vector3 distanceToAttacker = data.damageSource.transform.position - transform.position;
            distanceToAttacker.y = 0;

            if(Vector3.Angle(transform.forward,distanceToAttacker) > hitAngle * 0.5f)
            {
                return;
            }

            isInvulnerable = true;
            currentHitPoints -= data.amount;

            var messageType = currentHitPoints <= 0 ? MessageType.Dead : MessageType.Damaged;

            for(int i = 0; i < onDamageMessageReceivers.Count; i++)
            {
                var messageReceiver = onDamageMessageReceivers[i] as IMessageReceiver;
                messageReceiver.OnReceiveMessage(messageType, this, data);
            }

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
