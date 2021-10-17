using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {
    public class FixedUpdateFollow : MonoBehaviour
    {
        [SerializeField] Transform handTransform;
        // Start is called before the first frame update
        private void LateUpdate()
        {
            if (handTransform == null) return;
            transform.position = handTransform.position;
            transform.rotation = handTransform.rotation;
        }
    }
}

