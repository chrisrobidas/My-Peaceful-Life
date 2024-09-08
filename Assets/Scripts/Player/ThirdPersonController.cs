using Cinemachine;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;

/* Note: Animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    public class ThirdPersonController : NetworkBehaviour
    {
        [Header("References")]
        public SimpleKCC KCC;
        public StarterAssetsInputs StarterAssetsInputs;
        public Animator Animator;
        public Transform CameraPivot;

        [Header("Camera Setup")]
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;

        [Header("Movement Setup")]
        public float WalkSpeed = 2f;
        public float SprintSpeed = 5f;
        public float JumpImpulse = 10f;
        public float UpGravity = 25f;
        public float DownGravity = 40f;
        public float RotationSpeed = 8f;

        [Header("Movement Accelerations")]
        public float GroundAcceleration = 55f;
        public float GroundDeceleration = 25f;
        public float AirAcceleration = 25f;
        public float AirDeceleration = 1.3f;

        [Header("Sounds")]
        public AudioClip[] FootstepAudioClips;
        public AudioClip LandingAudioClip;
        [Range(0f, 1f)]
        public float FootstepAudioVolume = 0.5f;

        [Networked]
        private NetworkBool _isJumping { get; set; }

        private Vector3 _moveVelocity;

        // Cinemachine
        private Camera _mainCamera;
        private CinemachineVirtualCamera _virtualCamera;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private const float _threshold = 0.01f;

        // Animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        public override void FixedUpdateNetwork()
        {
            ProcessInput(StarterAssetsInputs.CurrentInput);

            if (KCC.IsGrounded)
            {
                // Stop jumping
                _isJumping = false;
            }
        }

        public override void Render()
        {
            Animator.SetFloat(_animIDSpeed, KCC.RealSpeed, 0.15f, Time.deltaTime);
            Animator.SetFloat(_animIDMotionSpeed, 1f);
            Animator.SetBool(_animIDJump, _isJumping);
            Animator.SetBool(_animIDGrounded, KCC.IsGrounded);
            Animator.SetBool(_animIDFreeFall, KCC.RealVelocity.y < -10f);
        }

        private void Awake()
        {
            AssignAnimationIDs();

            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _cinemachineTargetYaw = CameraPivot.transform.rotation.eulerAngles.y;

            _virtualCamera = GameObject.FindGameObjectWithTag(Constants.PLAYER_FOLLOW_CAMERA).GetComponent<CinemachineVirtualCamera>();

            if (HasInputAuthority)
            {
                _virtualCamera.Follow = CameraPivot;
            }
        }

        private void LateUpdate()
        {
            // Only local player needs to update the camera
            // Note: In shared mode the local player has always state authority over player's objects.
            if (HasStateAuthority == false)
                return;

            CameraRotation(StarterAssetsInputs.CurrentInput);
        }

        private void ProcessInput(GameplayInput input)
        {
            float jumpImpulse = 0f;

            // Comparing current input buttons to previous input buttons - this prevents glitches when input is lost
            if (KCC.IsGrounded && input.IsJumping)
            {
                // Set world space jump vector
                jumpImpulse = JumpImpulse;
                _isJumping = true;
            }

            // It feels better when the player falls quicker
            KCC.SetGravity(KCC.RealVelocity.y >= 0f ? UpGravity : DownGravity);

            float speed = input.IsSprinting ? SprintSpeed : WalkSpeed;

            Quaternion lookRotation = new Quaternion(0.0f, _mainCamera.transform.rotation.y, 0.0f, _mainCamera.transform.rotation.w);
            // Calculate correct move direction from input (rotated based on camera look)
            Vector3 moveDirection = lookRotation * new Vector3(input.MoveDirection.x, 0f, input.MoveDirection.y);
            moveDirection.Normalize();
            Vector3 desiredMoveVelocity = moveDirection * speed;

            float acceleration;
            if (desiredMoveVelocity == Vector3.zero)
            {
                // No desired move velocity - we are stopping
                acceleration = KCC.IsGrounded ? GroundDeceleration : AirDeceleration;
            }
            else
            {
                // Rotate the character towards move direction over time
                Quaternion currentRotation = KCC.TransformRotation;
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                Quaternion nextRotation = Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Runner.DeltaTime);

                KCC.SetLookRotation(nextRotation.eulerAngles);

                acceleration = KCC.IsGrounded ? GroundAcceleration : AirAcceleration;
            }

            _moveVelocity = Vector3.Lerp(_moveVelocity, desiredMoveVelocity, acceleration * Runner.DeltaTime);

            // Ensure consistent movement speed even on steep slope
            if (KCC.ProjectOnGround(_moveVelocity, out Vector3 projectedVector))
            {
                _moveVelocity = projectedVector;
            }

            Debug.Log("_moveVelocity: " + _moveVelocity);

            KCC.Move(_moveVelocity, jumpImpulse);
        }

        private void CameraRotation(GameplayInput input)
        {
            // If there is an input and camera position is not fixed
            if (input.LookRotation.sqrMagnitude >= _threshold)
            {
                // Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = StarterAssetsInputs.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += input.LookRotation.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += input.LookRotation.y * deltaTimeMultiplier;
            }

            // Clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CameraPivot.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        // Animation event
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f)
                return;

            if (FootstepAudioClips.Length > 0)
            {
                int index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], KCC.Position, FootstepAudioVolume);
            }
        }

        // Animation event
        private void OnLand(AnimationEvent animationEvent)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, KCC.Position, FootstepAudioVolume);
        }
    }
}