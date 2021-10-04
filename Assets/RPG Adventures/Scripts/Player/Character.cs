using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG { 
    public class Character : MonoBehaviour
    {
        [SerializeField] float playerMoveSpeed;
        [SerializeField] float playerRotationSpeed;
        
        Camera mainCam;
        CharacterInput input;
        CharacterController controller;
        Animator anim;

        float currentSpeed;
        float desiredSpeed;
        const float acceleration = 20f;
        const float decelaration = 100f;
        float accMultiplier;

        readonly int speedFloat = Animator.StringToHash("SpeedFloat");

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<CharacterInput>();
            anim = GetComponent<Animator>();
            mainCam = Camera.main;
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
            HandleRotation();
        }

        private void HandleRotation()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector3 direction = input.GetInput.normalized;
            accMultiplier = (input.IsInput) ? acceleration : decelaration;

            desiredSpeed = direction.magnitude * playerMoveSpeed;
            currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, Time.fixedDeltaTime * accMultiplier);
            
            anim.SetFloat(speedFloat, currentSpeed);
        }
    }
}