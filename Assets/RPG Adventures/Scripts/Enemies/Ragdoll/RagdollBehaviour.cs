using UnityEngine;

namespace RPG {
    public class RagdollBehaviour : MonoBehaviour
    {
        [SerializeField] float destroyTime = 6.0f;
        [SerializeField] Vector3 offset;

        private void Awake()
        {
            destroyTime += Time.time;;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time >= destroyTime)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
