using System;
using System.Collections;
using UnityEngine;

namespace RPG {
    public class CharacterInput : MonoBehaviour
    {

        [SerializeField] float attackRate = 0.3f;

        Vector3 input;
        bool canSprint;
        bool isAttacking;

        public Vector3 GetInput
        {
            get { 
                return input;
            }
        }

        public bool IsAttacking
        {
            get { return isAttacking; }
        }

        public bool IsInput
        {
            get
            {
                return !Mathf.Approximately(input.magnitude, 0f);
            }
        }

        public bool CanSprint
        {
            get
            {
                return canSprint;
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

            canSprint = Input.GetKey(KeyCode.LeftShift);

            if(Input.GetMouseButton(0) && !isAttacking)
            {
                StartCoroutine(HandleAttackInput());
            }
        }

        IEnumerator HandleAttackInput()
        {
            isAttacking = true;
            yield return new WaitForSeconds(attackRate);
            isAttacking = false;
        }
    }
}
