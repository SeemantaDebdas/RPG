using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG { 
    public class Character : MonoBehaviour
    {
        [SerializeField] float playerMoveSpeed;
        [SerializeField] float playerRotationSpeed;
        [SerializeField] Transform playerCamera;

        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            //MyMethodOfRotation();
            HandleRotation();
        }

        private void HandleRotation()
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            

            rb.MovePosition(rb.position + direction * playerMoveSpeed * Time.fixedDeltaTime);
        }

        private void MyMethodOfRotation()
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetForward = Quaternion.LookRotation(direction);
                Quaternion desiredForward = Quaternion.RotateTowards(transform.rotation, targetForward, playerRotationSpeed * Time.fixedDeltaTime);
                transform.rotation = desiredForward;

                rb.MovePosition(rb.position + direction * playerMoveSpeed * Time.fixedDeltaTime);
            }
        }
    }
}