using Cinemachine;
using Fusion;
using Fusion.Addons.KCC;
using System.Collections;
using UnityEngine;

public class ThirdPersonController : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private KCC KCC;
    [SerializeField] private PlayerInputs PlayerInputs;
    [SerializeField] private Animator Animator;
    [SerializeField] private Transform CameraPivot;
    [SerializeField] private GameObject ToolSocket;

    [Header("Camera Setup")]
    [SerializeField] private float TopClamp = 70.0f;
    [SerializeField] private float BottomClamp = -30.0f;

    [Header("Movement Setup")]
    [SerializeField] private float JumpImpulse = 6f;
    [SerializeField] private float RotationSpeed = 8f;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] FootstepAudioClips;
    [SerializeField] private AudioClip LandingAudioClip;
    [Range(0f, 1f)]
    [SerializeField] private float FootstepAudioVolume = 0.5f;

    [Header("Character customization")]
    public Material[] PlayerPrefabMaterials;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

    [Networked]
    private int _characterMaterialIndex { get; set; }
    [Networked]
    private NetworkBool _isJumping { get; set; }
    [Networked]
    private int _heldToolID { get; set; }

    // Cinemachine
    private Camera _mainCamera;
    private CinemachineVirtualCamera _virtualCamera;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private const float Threshold = 0.01f;

    // Animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    // Jump
    private bool _canJump;

    // Interaction
    private bool _isInteracting;

    public override void Spawned()
    {
        KCC.Collider.tag = KCC.tag;
        _skinnedMeshRenderer.material = PlayerPrefabMaterials[_characterMaterialIndex];
        DisplayHeldTool();
    }

    public override void FixedUpdateNetwork()
    {
        if (KCC.Data.IsGrounded)
        {
            _isJumping = false;
        }

        if (!_isInteracting)
        {
            ProcessInput(PlayerInputs.CurrentGameplayInput);
        }
        else
        {
            KCC.SetInputDirection(Vector3.zero);
        }
    }

    public override void Render()
    {
        Animator.SetFloat(_animIDSpeed, KCC.Data.RealSpeed, 0.15f, Time.deltaTime);
        Animator.SetFloat(_animIDMotionSpeed, 1f);
        Animator.SetBool(_animIDJump, _isJumping);
        Animator.SetBool(_animIDGrounded, KCC.Data.IsGrounded);
        Animator.SetBool(_animIDFreeFall, KCC.Data.RealVelocity.y < -5f);
    }

    public void SetCharacterMaterialIndex(int characterMaterialIndex)
    {
        _characterMaterialIndex = characterMaterialIndex;
        _skinnedMeshRenderer.material = PlayerPrefabMaterials[_characterMaterialIndex];
    }

    public void PlayInteractionAnimation(InteractionAnimation interactionAnimation, float duration)
    {
        if (_isInteracting) return;

        _isInteracting = true;
        Animator.SetTrigger(interactionAnimation.ToString());
        StartCoroutine(InteractionCooldown(duration));
    }

    private IEnumerator InteractionCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isInteracting = false;
    }

    public void SwapHeldTool(int toolID)
    {
        if (_heldToolID == toolID) return;

        _heldToolID = toolID;

        RPC_SwapHeldTool();
    }


    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SwapHeldTool()
    {
        DisplayHeldTool();

        // Play some animation and particle effect..
    }

    private void DisplayHeldTool()
    {
        for (int i = 0; i < ToolSocket.transform.childCount; i++)
        {
            if (i == _heldToolID)
            {
                ToolSocket.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                ToolSocket.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
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

        CameraRotation(PlayerInputs.CurrentGameplayInput);
    }

    private void ProcessInput(GameplayInput input)
    {
        if (KCC.Data.IsGrounded)
        {
            if (input.IsJumping)
            {
                if (_canJump)
                {
                    KCC.Jump(Vector3.up * JumpImpulse);
                    _isJumping = true;
                    _canJump = false;
                }
            }
            else
            {
                _canJump = true;
            }
        }

        // Calculate correct move direction from input (rotated based on camera look)
        Quaternion lookRotation = new Quaternion(0.0f, _mainCamera.transform.rotation.y, 0.0f, _mainCamera.transform.rotation.w);
        Vector3 moveDirection = lookRotation * new Vector3(input.MoveDirection.x, 0f, input.MoveDirection.y);
        KCC.SetInputDirection(moveDirection);

        // Applies the sprint modifier
        KCC.SetIsSprinting(input.IsSprinting);

        // Rotate the character towards move direction over time
        if (moveDirection != Vector3.zero)
        {
            Quaternion currentRotation = KCC.Data.TransformRotation;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion nextRotation = Quaternion.Lerp(currentRotation, targetRotation, RotationSpeed * Runner.DeltaTime);
            KCC.SetLookRotation(nextRotation.eulerAngles);
        }
    }

    private void CameraRotation(GameplayInput input)
    {
        // If there is an input and camera position is not fixed
        if (input.LookRotation.sqrMagnitude >= Threshold)
        {
            // Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = PlayerInputs.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

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
        // TODO: CHECK IF FOOTSTEP SOUND IS FIXED?? PROBABLY SINCE I FIXED JUMP ANIMATION?
        if (animationEvent.animatorClipInfo.weight < 0.5f)
            return;

        if (FootstepAudioClips.Length > 0)
        {
            int index = Random.Range(0, FootstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(FootstepAudioClips[index], KCC.Transform.position, FootstepAudioVolume);
        }
    }

    // Animation event
    private void OnLand(AnimationEvent animationEvent)
    {
        AudioSource.PlayClipAtPoint(LandingAudioClip, KCC.Transform.position, FootstepAudioVolume);
    }
}
