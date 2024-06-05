using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;

    [SerializeField] Transform _orientation;
    [SerializeField] Transform _playerObj;
    [SerializeField] Transform _camTarget;
    public Transform CamTarget
    { get { return _camTarget; } }

    public float mouseSensitivity;

    Vector2 _camInput;

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

        _playerObj.transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
