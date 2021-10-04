using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float timeToLerp = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float currentRotationY = transform.eulerAngles.y;
        float wantedRotationY = target.eulerAngles.y;

        currentRotationY = Mathf.LerpAngle(currentRotationY, wantedRotationY, timeToLerp);

        transform.position = new Vector3(target.position.x,
                                        5f,
                                        target.position.z);

        Quaternion modRotation = Quaternion.Euler(0, currentRotationY, 0);
        Vector3 rotatedPosition = modRotation * Vector3.forward;
        transform.position -= rotatedPosition * 10;
        transform.LookAt(target);

    }
}
