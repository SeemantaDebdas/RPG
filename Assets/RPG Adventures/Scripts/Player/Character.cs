using UnityEngine;
using Cinemachine;

namespace RPG { 
    public class Character : MonoBehaviour
    {
        [SerializeField] float playerMoveSpeed;
        [SerializeField] float playerRotationSpeed;

        CinemachineCam cinemachineCam;
        CharacterInput input;
        Animator anim;

        float currentSpeed;
        float desiredSpeed;
        const float acceleration = 20f;
        const float decelaration = 100f;
        float accMultiplier;

        readonly int speedFloat = Animator.StringToHash("SpeedFloat");

        private void Awake()
        {
            input = GetComponent<CharacterInput>();
            anim = GetComponent<Animator>();
            cinemachineCam = GetComponent<CinemachineCam>();
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
            HandleMovement();
            HandleRotation();
        }

        private void HandleRotation()
        {
            //The rotation main camera is making with world V3 forward
            Vector3 cameraRotationWithWorld = Quaternion.Euler(0, cinemachineCam.cineCam.m_XAxis.Value, 0) * Vector3.forward;

            //find out the rotation that the direction vector makes with world V3 forward
            Vector3 direction = input.GetInput.normalized;
            Quaternion moveRotation = Quaternion.FromToRotation(Vector3.forward, direction);

            Quaternion targetRotation;
            if (Mathf.Approximately(Vector3.Dot(Vector3.forward, direction), -1.0f))
            {
                targetRotation = Quaternion.LookRotation(-cameraRotationWithWorld);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(moveRotation * cameraRotationWithWorld);
            }
            

            if (input.IsInput)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, playerRotationSpeed * Time.fixedDeltaTime);
            }

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