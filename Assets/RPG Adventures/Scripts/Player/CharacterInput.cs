using UnityEngine;

namespace RPG {
    public class CharacterInput : MonoBehaviour
    {
        Vector3 input;
        public Vector3 GetInput
        {
            get { 
                return input;
            }
        }

        public bool IsInput
        {
            get
            {
                return !Mathf.Approximately(input.magnitude, 0f);
            }
        }
        // Update is called once per frame
        void Update()
        {
            input.Set(
                      Input.GetAxis("Horizontal"),
                      0,
                      Input.GetAxis("Vertical")
                      );
        }
    }
}
