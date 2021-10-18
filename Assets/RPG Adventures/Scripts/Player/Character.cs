using UnityEngine;
using Cinemachine;

namespace RPG { 
    public class Character : MonoBehaviour,IAttackAnimListener,IMessageReceiver
    {
        public static Character Instance
        {
            get
            {
                return instance;
            }
        }

        public bool IsRespawning { get { return isRespawning; } }

        static Character instance;

        [SerializeField] Weapon weapon;
        [SerializeField] Transform handTransform;

        [Header("General Parameters")]
        [SerializeField] float playerMoveSpeed;
        [SerializeField] float playerRotationSpeed;
        [SerializeField] float gravity = 10f;

        HudManager hudManager;
        CharacterController controller;
        Damageable damagable;
        CinemachineCam cinemachineCam;
        CharacterInput input;
        Animator anim;

        float currentVerticalSpeed;
        float currentSpeed;
        float desiredSpeed;
        const float acceleration = 20f;
        const float decelaration = 10f;
        const float walkSpeed = 5f;
        const float runSpeed = 14f;
        float accMultiplier;

        readonly int speedFloat = Animator.StringToHash("SpeedFloat");
        readonly int attackTrigger = Animator.StringToHash("AttackTrigger");
        readonly int deathTrigger = Animator.StringToHash("DeathTrigger");
        readonly int blockInputTag = Animator.StringToHash("BlockInput");

        AnimatorStateInfo currentAnimation;
        AnimatorStateInfo nextAnimation;
        bool isTransitioning;

        bool isRespawning;

        bool inputBlocked;

        private void Awake()
        {
            instance = this;

            controller = GetComponent<CharacterController>();
            input = GetComponent<CharacterInput>();
            anim = GetComponent<Animator>();
            cinemachineCam = Camera.main.GetComponent<CinemachineCam>();
            damagable = GetComponent<Damageable>();
        }

        private void Start()
        {
            hudManager = FindObjectOfType<HudManager>();
            hudManager.SetMaxHealth(damagable.maxHitPoints);
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleRotation();
            HandleAttack();
            HandleGravity();

            CacheAnimation();
            ProcessBlockInput();
        }

        private void OnAnimatorMove()
        {
            if (isRespawning) return;
            Vector3 movement = anim.deltaPosition;
            movement += currentVerticalSpeed * Vector3.up * Time.fixedDeltaTime;
            controller.Move(movement);
        }

        void HandleGravity()
        {
            currentVerticalSpeed = -gravity;
        }

        private void HandleRotation()
        {
            //The rotation main camera is making with world V3 forward
            Vector3 cameraRotationWithWorld = Quaternion.Euler(0, cinemachineCam.CineCam.m_XAxis.Value, 0) * Vector3.forward;

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
            playerMoveSpeed = (input.CanSprint) ? runSpeed : walkSpeed;

            desiredSpeed = direction.magnitude * playerMoveSpeed;
            currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, Time.fixedDeltaTime * accMultiplier);
            
            anim.SetFloat(speedFloat, currentSpeed);
        }

        void HandleAttack()
        {
            anim.ResetTrigger(attackTrigger);
            if (input.IsAttacking)
            {
                anim.SetTrigger(attackTrigger);
            }
        }

        public void MeleeAttackStart()
        {
            if(weapon!=null)
                weapon.BeginAttack();
        }

        public void MeleeAttackStop()
        {
            if (weapon != null)
                weapon.StopAttack();
        }

        public void EquipWeapon(InventorySlot slot)
        {
            if (weapon != null)
            {
                if (slot.item.name == weapon.name) return;
                else
                {
                    Destroy(weapon.gameObject);
                }
            }
            weapon = Instantiate(slot.item, transform).GetComponent<Weapon>();
            weapon.SetOwner(this.gameObject);
            weapon.name = slot.item.name;
            weapon.GetComponent<FixedUpdateFollow>().SetFollowTransform(handTransform);
        }

        public void OnReceiveMessage(MessageType type, object damageable, object message)
        {
            if(type == MessageType.Damaged)
            {
                hudManager.SetHealth((damageable as Damageable).currentHitPoints);
            }
            else if(type == MessageType.Dead)
            {
                anim.SetTrigger(deathTrigger);
                hudManager.SetHealth(0);
                isRespawning = true;
            }
        }

        void CacheAnimation()
        {
            currentAnimation = anim.GetCurrentAnimatorStateInfo(0);
            nextAnimation = anim.GetNextAnimatorStateInfo(0);
            isTransitioning = anim.IsInTransition(0);
        }

        void ProcessBlockInput()
        {
            inputBlocked = currentAnimation.tagHash == blockInputTag && !isTransitioning;
            inputBlocked |= nextAnimation.tagHash == blockInputTag;
            input.IsInputBlocked = inputBlocked;
        }

        public void StartRespawn()
        {
            transform.position = Vector3.zero;
            hudManager.SetMaxHealth(damagable.maxHitPoints);
            damagable.SetInitialHealth();
        }

        public void FinishRespawn()
        {
            isRespawning = false;
        }
    }
}