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

    Vector2 _camInput;

    Vector3 inputDir;

    float _yRotation;
    float _xRotation;

    [SerializeField] float _minXRotation;
    [SerializeField] float _maxXRotation;

    private void OnEnable()
    {
        _playerInput.actions.FindAction("Camera").started += OnCamera;
        _playerInput.actions.FindAction("Camera").performed += OnCamera;
        _playerInput.actions.FindAction("Camera").canceled += OnCamera;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Camera").started -= OnCamera;
        _playerInput.actions.FindAction("Camera").performed -= OnCamera;
        _playerInput.actions.FindAction("Camera").canceled -= OnCamera;
    }


    void OnCamera(InputAction.CallbackContext context)
    {
        _camInput = context.ReadValue<Vector2>();
    }

    void Update()
    {


        if (_playerInput.currentActionMap == _playerInput.actions.FindActionMap("Menu"))
        {
            return;
        }

        _orientation.forward = _playerObj.forward.normalized;

        float mouseY = _camInput.y * mouseSensitivity;
        float mouseX = _camInput.x * mouseSensitivity;

        _yRotation += mouseX;
        _xRotation -= mouseY;

        _xRotation = Mathf.Clamp(_xRotation, -_minXRotation, _maxXRotation);

        _camTarget.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);

        Vector3 viewDir = _stateMachine.transform.position - new Vector3(transform.position.x, _stateMachine.transform.position.y, transform.position.z);
        _orientation.forward = viewDir.normalized;

        inputDir = _orientation.forward * _stateMachine.CurrentMovementInput.y + _orientation.right * _stateMachine.CurrentMovementInput.x;

        Quaternion lookRotation = Quaternion.LookRotation(inputDir, Vector3.up);
        _playerObj.transform.rotation = Quaternion.Slerp(_playerObj.transform.rotation, lookRotation, Time.deltaTime * _playerRotationSpeed);
        // _playerObj.transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
