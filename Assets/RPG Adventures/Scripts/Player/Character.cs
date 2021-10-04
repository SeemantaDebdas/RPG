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

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<CharacterInput>();
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
            Vector3 direction = input.GetInput;
            Quaternion cameraRotation = mainCam.transform.rotation;

            Vector3 targetDirection = cameraRotation * direction;
            targetDirection.y = 0;

            controller.Move(targetDirection.normalized * playerMoveSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0);
        }
    }
}