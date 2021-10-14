using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {
    public class ReplaceWithRagdoll : MonoBehaviour
    {
        [SerializeField] GameObject ragdollPrefab;
        public void Replace()
        {
            GameObject ragdollPrefabSpawn = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}

