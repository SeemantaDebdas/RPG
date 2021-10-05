using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float detectionRange = 10f;
        [SerializeField] float detectionAngle = 90f;

        Transform playerTransform;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            DetectPlayer();
        }

        void DetectPlayer()
        {
            if (!Character.Instance) return;
            playerTransform = Character.Instance.transform;
            Vector3 distanceToPlayer = playerTransform.position - transform.position;
            distanceToPlayer.y = 0f;

            if (distanceToPlayer.magnitude <= detectionRange)
            {
                if(Vector3.Dot(distanceToPlayer.normalized,transform.forward)>
                    Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
                {
                    Debug.Log("Player Detected");
                }
            }
            else
            {
                Debug.Log("Player Lost");
            }
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

