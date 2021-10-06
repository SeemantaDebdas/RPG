using RPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScanner 
{
    public float detectionRange = 10f;
    public float detectionAngle = 90f;

    public Character Detect(Transform detector)
    {
        if (Character.Instance == null) return null;

        Vector3 distanceToPlayer = Character.Instance.transform.position - detector.position;
        distanceToPlayer.y = 0f;

        if (distanceToPlayer.magnitude <= detectionRange)
        {
            if (Vector3.Dot(distanceToPlayer.normalized, detector.forward) >
                Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                return Character.Instance;
            }
        }

        return null;
    }
}
