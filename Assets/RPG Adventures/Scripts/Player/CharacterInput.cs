using System;
using System.Collections;
using UnityEngine;

namespace RPG {
    public class CharacterInput : MonoBehaviour
    {

        [SerializeField] float attackRate = 0.3f;
        [SerializeField] float interactableDistance = 10f;

        Vector3 movementInput;
        bool canSprint;
        bool isAttacking;
        bool isLeftMouseClick;
        bool isInteracting;
        bool isTalking;

        public Vector3 GetInput { get { return movementInput; } }
        public bool IsAttacking { get { return isAttacking; } }
        public bool IsInput { get { return !Mathf.Approximately(movementInput.magnitude, 0f); } }
        public bool CanSprint { get { return canSprint; } }


        // Update is called once per frame
        void Update()
        {
            movementInput.Set(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

            canSprint = Input.GetKey(KeyCode.LeftShift);

            isLeftMouseClick = Input.GetMouseButton(0);
            if (isLeftMouseClick)
            {
                HandleLeftMouseClick();
            }

            isInteracting = Input.GetKeyDown(KeyCode.E);
            if (isInteracting)
            {
                HandleInteraction();
            }

        }

        private void HandleInteraction()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out RaycastHit hit, interactableDistance);
            if (isHit && hit.collider.CompareTag("QuestGiver"))
            {
                isTalking = true;
            }
        }

        private void HandleLeftMouseClick()
        {
            if (!isAttacking)
                StartCoroutine(HandleAttackInput());
        }

        IEnumerator HandleAttackInput()
        {
            isAttacking = true;
            yield return new WaitForSeconds(attackRate);
            isAttacking = false;
        }
    }
}
