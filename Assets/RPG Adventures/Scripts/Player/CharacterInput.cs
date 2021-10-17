using System;
using System.Collections;
using UnityEngine;

namespace RPG {
    public class CharacterInput : MonoBehaviour
    {
        static CharacterInput instance;

        [SerializeField] float attackRate = 0.3f;

        Vector3 movementInput;
        bool canSprint;
        bool isAttacking;
        bool isLeftMouseClick;
        bool isInteracting;
        bool uIActive;

        Collider hitTarget;

        public static CharacterInput Instance{get { return instance; }}
        public Vector3 GetInput { get { return movementInput; } }
        public bool IsAttacking { get { return isAttacking; } }
        public bool IsInput { get { return !Mathf.Approximately(movementInput.magnitude, 0f); } }
        public bool CanSprint { get { return canSprint; } }
        public Collider HitTarget { get { return hitTarget; } }
        public bool UIActive { get { return uIActive; }set { uIActive = value; } }

        private void Awake()
        {
            instance = this;
        }

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
            bool isHit = Physics.Raycast(ray, out RaycastHit hit);
            if (isHit)
            {
                StartCoroutine(HandleHitTarget(hit.collider));
            }
        }

        IEnumerator HandleHitTarget(Collider hitCollider)
        {
            hitTarget = hitCollider;
            yield return new WaitForSeconds(0.5f);
            hitTarget = null;
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
