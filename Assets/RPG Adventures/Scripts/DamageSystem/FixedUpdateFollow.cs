using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {
    public class FixedUpdateFollow : MonoBehaviour
    {
        [SerializeField] Transform handTransform;
        bool isHandled=false;
        // Start is called before the first frame update
        void LateUpdate()
        {
            if (isHandled)
            {
                transform.localPosition = handTransform.position;
                transform.localRotation = handTransform.rotation;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<BoxCollider>().enabled = false;
                isHandled = true;
            }
        }
    }
}

