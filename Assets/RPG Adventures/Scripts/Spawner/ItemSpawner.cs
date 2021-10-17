using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG {
    public class ItemSpawner : MonoBehaviour
    {
        public UnityEvent<ItemSpawner> onItemPickup;
        public GameObject weaponPrefab;
        public Sprite weaponImageSprite;
        [SerializeField] LayerMask targetLayerMask;

        // Start is called before the first frame update
        void Awake()
        {
            Instantiate(weaponPrefab, transform);
            Destroy(transform.GetChild(0).gameObject);

            onItemPickup.AddListener(FindObjectOfType<InventoryManager>().OnItemPickup);
        }

        private void OnTriggerEnter(Collider other)
        {
            if((targetLayerMask.value & 1<<other.gameObject.layer) != 0)
            {
                onItemPickup.Invoke(this);
                Destroy(this.gameObject);
            }
        }
    }
}
