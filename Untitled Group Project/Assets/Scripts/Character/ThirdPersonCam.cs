using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;

    [SerializeField] CharStateMachine _stateMachine;

    [SerializeField] Transform _orientation;
    [SerializeField] Transform _playerObj;

    [SerializeField] Transform _camTarget;
    public Transform CamTarget
    { get { return _camTarget; } }

    public float mouseSensitivity;

    [SerializeField] float _playerRotationSpeed;


    Vector2 cam;
    float camY_;

    Vector3 inputDir;

    float _yRotation;
    float _xRotation;

    [SerializeField] float _minXRotation;
    [SerializeField] float _maxXRotation;

    private void OnEnable()
    {
        _playerInput.actions.FindAction("Mouse").started += OnCamera;
        _playerInput.actions.FindAction("Mouse").performed += OnCamera;
        _playerInput.actions.FindAction("Mouse").canceled += OnCamera;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Mouse").started -= OnCamera;
        _playerInput.actions.FindAction("Mouse").performed -= OnCamera;
        _playerInput.actions.FindAction("Mouse").canceled -= OnCamera;
    }


    void OnCamera(InputAction.CallbackContext context)
    {
        cam = context.ReadValue<Vector2>();
    }

    void Update()
    {


        if (_playerInput.currentActionMap == _playerInput.actions.FindActionMap("Menu"))
        {
            return;
        }

        _orientation.forward = _playerObj.forward.normalized;

        float mouseY = cam.y * mouseSensitivity * Time.deltaTime;
        float mouseX = cam.x * mouseSensitivity * Time.deltaTime;

        // float inputx = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // float inputy = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;

        _xRotation = Mathf.Clamp(_xRotation, -_minXRotation, _maxXRotation);

        _camTarget.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);



        // _playerObj.transform.rotation = Quaternion.Slerp(_playerObj.transform.rotation, lookRotation, Time.deltaTime * _playerRotationSpeed);
    }
}

// _orientation.forward = viewDir.normalized;
// Vector3 viewDir = _stateMachine.transform.position - new Vector3(transform.position.x, _stateMachine.transform.position.y, transform.position.z);
// Quaternion lookRotation = Quaternion.LookRotation(inputDir, Vector3.up);
// inputDir = _orientation.forward * _stateMachine.CurrentMovementInput.y + _orientation.right * _stateMachine.CurrentMovementInput.x;