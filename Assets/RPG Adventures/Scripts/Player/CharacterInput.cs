using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        bool isInputBlocked;

        Collider hitTarget;

        public static CharacterInput Instance{get { return instance; }}
        public Vector3 GetInput { 
            get {
                if (isInputBlocked) return Vector3.zero;
                return movementInput; 
            } 
        }

        public bool IsAttacking { get { return !isInputBlocked && isAttacking; } }
        public bool IsInput { get { return !Mathf.Approximately(movementInput.magnitude, 0f) && !isInputBlocked; } }
        public bool CanSprint { get { return canSprint; } }
        public Collider HitTarget { get { return hitTarget; } }
        public bool IsInputBlocked { get { return isInputBlocked; }set { isInputBlocked = value; } }

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
            if (!isAttacking && !IsPointerOverUIElement())
                StartCoroutine(HandleAttackInput());
        }

        bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }

        IEnumerator HandleAttackInput()
        {
            isAttacking = true;
            yield return new WaitForSeconds(attackRate);
            isAttacking = false;
        }
    }
}
