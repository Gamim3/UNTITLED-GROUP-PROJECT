using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharStateMachine : Entity
{
    //CLEAN UP CODE
    #region Variables

    [Header("Refrences")]
    #region Refrences

    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] Transform _playerObj;
    public Transform PlayerObj
    { get { return _playerObj; } }

    [SerializeField] Animator _playerAnimator;
    public Animator PlayerAnimator
    { get { return _playerAnimator; } }

    [SerializeField] Transform _playerCam;
    public Transform PlayerCam
    { get { return _playerCam; } }

    [SerializeField] private Transform _orientation;
    public Transform Orientation
    { get { return _orientation; } }

    [SerializeField] Rigidbody _playerRigidBody;
    public Rigidbody PlayerRigidBody
    { get { return _playerRigidBody; } }

    CharStateFactory _states;

    CharBaseState _currentState;

    public CharBaseState CurrentState
    { get { return _currentState; } set { _currentState = value; } }

    #endregion

    [Header("Movement")]
    #region Movement

    [SerializeField] Vector2 _currentMovementInput;
    public Vector2 CurrentMovementInput
    { get { return _currentMovementInput; } }

    [SerializeField] Vector3 _currentMovement;
    public Vector3 CurrentMovement
    { get { return _currentMovement; } set { _currentMovement = value; } }

    [SerializeField] Vector3 _movement;
    public Vector3 Movement
    { get { return _movement; } set { _movement = value; } }

    [SerializeField] float _movementSpeed;
    public float MovementSpeed
    { get { return _movementSpeed; } }

    [SerializeField] float _moveForce;
    public float MoveForce
    { get { return _moveForce; } set { _moveForce = value; } }

    [SerializeField] float _desiredMoveForce;
    public float DesiredMoveForce
    { get { return _desiredMoveForce; } set { _desiredMoveForce = value; } }

    [SerializeField] float _lastDesiredMoveForce;
    public float LastDesiredMoveForce
    { get { return _lastDesiredMoveForce; } set { _lastDesiredMoveForce = value; } }

    #endregion

    [Header("Stamina")]    // Inherits Energy From Entity so it should not need its own stamina
    #region Stamina


    // [SerializeField] float _stamina;
    // public float Stamina
    // { get { return _stamina; } set { _stamina = value; } }

    // [SerializeField] float _maxStamina;
    // public float MaxStamina
    // { get { return _maxStamina; } }

    // [SerializeField] float _staminaDecreaseMultiplier;

    #endregion

    [Header("Jumping")]
    #region Jumping

    [SerializeField] int _jumpAmount;
    public int JumpAmount
    { get { return _jumpAmount; } set { _jumpAmount = value; } }

    [SerializeField] float _jumpForce;
    public float JumpForce
    { get { return _jumpForce; } }

    [SerializeField] Vector3 _jumpMent;
    public Vector3 JumpMent
    { get { return _jumpMent; } set { _jumpMent = value; } }

    [SerializeField] float _maxJumpTime;
    public float MaxJumpTime
    { get { return _maxJumpTime; } }

    [SerializeField] float _isJumpTime;
    public float IsJumpTime
    { get { return _isJumpTime; } set { _isJumpTime = value; } }

    #endregion

    [Header("Groundcheck")]
    #region GroundCheck

    [SerializeField] bool _isGrounded;
    public bool IsGrounded
    { get { return _isGrounded; } set { _isGrounded = value; } }

    [SerializeField] LayerMask _groundLayer;

    [SerializeField] float _groundDrag;
    public float GroundDrag
    { get { return _groundDrag; } }

    [SerializeField] float sphereRadius;
    [SerializeField] float sphereOffset;

    #endregion

    [Header("SlopeCheck")]
    #region SlopeCheck

    [SerializeField] bool _isSloped;
    public bool IsSloped
    { get { return _isSloped; } set { _isSloped = value; } }

    [SerializeField] bool _isExitingSlope;
    public bool IsExitingSlope
    { get { return _isExitingSlope; } set { _isExitingSlope = value; } }

    public RaycastHit _slopeHit;

    [SerializeField] float _maxSlopeAngle;

    [SerializeField] float _playerHeight;

    #endregion

    [Header("Inputs")]
    #region Inputs

    [SerializeField] bool _isMoveAction;
    public bool IsMoveAction
    { get { return _isMoveAction; } }

    [SerializeField] bool _isJumpAction;
    public bool IsJumpAction
    { get { return _isJumpAction; } }

    [SerializeField] bool _isRunAction;
    public bool IsRunAction
    { get { return _isRunAction; } set { _isRunAction = value; } }

    [SerializeField] bool _isAttack1Action;
    public bool IsAttack1Action
    { get { return _isAttack1Action; } }

    [SerializeField] bool _isAttack2Action;
    public bool IsAttack2Action
    { get { return _isAttack2Action; } }

    #endregion

    [Header("Settings")]
    #region Settings

    [SerializeField] bool _toggleRun;
    public bool ToggleRun
    { get { return ToggleRun1; } }

    #endregion

    [Header("Speeds")]
    #region Speeds

    [SerializeField] float _walkSpeed;
    public float WalkSpeed
    { get { return _walkSpeed; } }

    [SerializeField] float _runSpeed;
    public float RunSpeed
    { get { return _runSpeed; } }

    [SerializeField] float _airSpeed;
    public float AirSpeed
    { get { return _airSpeed; } }

    #endregion

    [Header("Speed Multipliers")]
    #region Speed Multipliers

    [SerializeField] float _speedIncreaseMultiplier;
    public float SpeedIncreaseMultiplier
    { get { return _speedIncreaseMultiplier; } }

    [SerializeField] float _moveMultiplier;
    public float MoveMultiplier
    { get { return _moveMultiplier; } set { _moveMultiplier = value; } }

    [SerializeField] float SlopeSpeedIncreaseMultiplier;

    [SerializeField] float _strafeSpeedMultiplier;
    public float StrafeSpeedMultiplier
    { get { return _strafeSpeedMultiplier; } set { _strafeSpeedMultiplier = value; } }

    #endregion

    [Header("States")]
    #region States

    [SerializeField] bool _isAired;
    public bool IsAired
    { get { return _isAired; } set { _isAired = value; } }

    [SerializeField] bool _isJumping;
    public bool IsJumping
    { get { return _isJumping; } set { _isJumping = value; } }

    #endregion

    [Header("ExtraForce")]
    #region ExtraForce

    [SerializeField] bool _isForced;
    public bool IsForced
    { get { return _isForced; } set { _isForced = value; } }

    [SerializeField] float _extraForce;
    public float ExtraForce
    { get { return _extraForce; } set { _extraForce = value; } }

    [SerializeField] float _forceSlowDownRate;
    public float ForceSlowDownRate
    { get { return _forceSlowDownRate; } set { _forceSlowDownRate = value; } }

    [SerializeField] bool _hasDied;
    public bool HasDied
    {
        get { return _hasDied; }
        set { _hasDied = value; }
    }

    public bool ToggleRun1 { get => _toggleRun; set => _toggleRun = value; }
    public bool ToggleRun2 { get => _toggleRun; set => _toggleRun = value; }

    #endregion

    #endregion

    private void Awake()
    {
        // DontDestroyOnLoad(this); 

        playerInput.actions.FindAction("Move").started += OnMovement;
        playerInput.actions.FindAction("Move").performed += OnMovement;
        playerInput.actions.FindAction("Move").canceled += OnMovement;

        playerInput.actions.FindAction("Jump").started += OnJump;
        playerInput.actions.FindAction("Jump").performed += OnJump;
        playerInput.actions.FindAction("Jump").canceled += OnJump;

        playerInput.actions.FindAction("Run").started += OnRun;
        playerInput.actions.FindAction("Run").performed += OnRun;
        playerInput.actions.FindAction("Run").canceled += OnRun;

        playerInput.actions.FindAction("Attack1").started += OnAttack1;
        playerInput.actions.FindAction("Attack1").performed += OnAttack1;
        playerInput.actions.FindAction("Attack1").canceled += OnAttack1;

        playerInput.actions.FindAction("Attack2").started += OnAttack2;
        playerInput.actions.FindAction("Attack2").performed += OnAttack2;
        playerInput.actions.FindAction("Attack2").canceled += OnAttack2;

        _states = new CharStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        _playerAnimator.SetTrigger("Grounded");

        _isGrounded = true;

        MoveForce = DesiredMoveForce;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _playerCam = FindObjectOfType<Camera>().transform;
    }

    public override void Update()
    {
        base.Update();

        _movementSpeed = PlayerRigidBody.velocity.magnitude;

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(_currentState.ToString());
        }

        CurrentMovement = (Orientation.forward * CurrentMovementInput.y) + (Orientation.right * CurrentMovementInput.x); // NORMALIZE MAYBE?

        _currentState.UpdateStates();

        IsGrounded = CheckGrounded();

        IsSloped = CheckSloped();

        if (IsGrounded || IsSloped)
        {
            PlayerRigidBody.drag = GroundDrag;
        }
        else if (!IsSloped && !IsGrounded)
        {
            PlayerRigidBody.drag = 0;
        }

        HandleStrafeSpeed();
        SpeedControl();

        if (Mathf.Abs(DesiredMoveForce - LastDesiredMoveForce) > 0f && MoveForce != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoovMoov());
        }
        else
        {
            MoveForce = DesiredMoveForce;
        }

        LastDesiredMoveForce = DesiredMoveForce;
    }

    #region MonoBehaviours

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    #endregion

    #region InputVoids

    void OnMovement(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMoveAction = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        _isJumpAction = context.ReadValueAsButton();
    }

    void OnRun(InputAction.CallbackContext context)
    {
        if (ToggleRun1 && context.ReadValueAsButton() == true)
        {
            _isRunAction = true;
        }
        else if (!ToggleRun1)
        {
            _isRunAction = context.ReadValueAsButton();
        }
    }

    void OnAttack1(InputAction.CallbackContext context)
    {
        _isAttack1Action = context.ReadValueAsButton();
    }

    void OnAttack2(InputAction.CallbackContext context)
    {
        _isAttack2Action = context.ReadValueAsButton();
    }

    #endregion

    #region STUFF

    private bool CheckGrounded()
    {
        Vector3 characterPosition = transform.position;

        Vector3 sphereCenter = characterPosition + Vector3.down * sphereOffset;
        bool isOnGround = Physics.SphereCast(sphereCenter, sphereRadius, Vector3.down, out RaycastHit hit, sphereOffset + 0.1f, _groundLayer);

        if (isOnGround)
        {
            Vector3 rayStart = characterPosition;
            Vector3 rayDirection = hit.point - rayStart;
            float rayDistance = Vector3.Distance(hit.point, rayStart) + 0.001f;

            if (Physics.Raycast(rayStart, rayDirection, out RaycastHit rayHit, rayDistance, _groundLayer))
            {
                if (CheckSloped())
                {
                    return true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool CheckSloped()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.8f, _groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        Debug.DrawRay(this.transform.position, Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized);
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }

    #endregion

    public void HandleStrafeSpeed()
    {
        if (_currentMovementInput.y == 1)
        {
            _strafeSpeedMultiplier = 1.0f; // Full speed when moving forward
        }
        else
        {
            // Adjust strafe speed based on forward input
            _strafeSpeedMultiplier = Mathf.Lerp(0.5f, 1.0f, Mathf.Abs(_currentMovementInput.y));
        }
    }

    private void SpeedControl()
    {
        if (IsSloped && !IsExitingSlope)
        {
            if (PlayerRigidBody.velocity.magnitude > MoveForce)
            {
                PlayerRigidBody.velocity = PlayerRigidBody.velocity.normalized * MoveForce;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(PlayerRigidBody.velocity.x, 0f, PlayerRigidBody.velocity.z);

            if (IsForced && flatVel.magnitude > ExtraForce)
            {
                Vector3 limitedVel = flatVel.normalized * ExtraForce;
                PlayerRigidBody.velocity = new Vector3(limitedVel.x, PlayerRigidBody.velocity.y, limitedVel.z);
            }
            else if (IsForced && flatVel.magnitude <= 1)
            {
                ExtraForce = 0;
            }

            if (IsForced)
            {
                ExtraForce -= _forceSlowDownRate * Time.deltaTime;

                if (ExtraForce <= MoveForce)
                {
                    IsForced = false;
                    ExtraForce = 0;
                }
            }

            if (!IsForced && flatVel.magnitude > MoveForce)
            {
                Vector3 limitedVel = flatVel.normalized * MoveForce;
                PlayerRigidBody.velocity = new Vector3(limitedVel.x, PlayerRigidBody.velocity.y, limitedVel.z);
            }
        }
    }

    IEnumerator SmoovMoov()
    {
        float time = 0;
        float difference = Mathf.Abs(DesiredMoveForce - MoveForce);
        float startValue = MoveForce;

        while (time < difference)
        {
            MoveForce = Mathf.Lerp(startValue, DesiredMoveForce, time / difference);

            if (CheckSloped())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * SpeedIncreaseMultiplier * SlopeSpeedIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * SpeedIncreaseMultiplier;
            }

            yield return null;
        }

        MoveForce = DesiredMoveForce;
    }

    #region Entity

    // public override TakeDamage(float damage)
    // {
    //     healthPoints -= damage;
    // }

    // public void Exhaustion(float Energy)
    // {
    //     energy -= (Energy / 50);
    // }

    #endregion
}