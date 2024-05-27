using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [SerializeField]
    Transform _orientation,
    _player,
    _camHolder,
    _playerObj;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private CharStateMachine _stateMachine;

    float inputY;
    float inputX;

    float oldInputY;
    float oldInputX;


    Vector3 inputDir;

    Quaternion currentCameraRotation;
    Quaternion previousCameraRotation;

    [SerializeField] float _rotationThreshold;


    [SerializeField] List<GameObject> _cinemachineCams = new List<GameObject>();

    private void Start()
    {
        // DontDestroyOnLoad(_camHolder);
        _stateMachine = FindObjectOfType<CharStateMachine>();

        _cinemachineCams.Add(GameObject.FindGameObjectWithTag("CineMachine"));

        for (int i = 0; i < _cinemachineCams.Count; i++)
        {
            // DontDestroyOnLoad(_cinemachineCams[i]);
        }

        _orientation = _stateMachine.Orientation;
        _player = _stateMachine.transform;
        _playerObj = _stateMachine.PlayerObj;
    }

    void Update()
    {
        Vector3 viewDir = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
        _orientation.forward = viewDir.normalized;

        inputDir = _orientation.forward * _stateMachine.CurrentMovementInput.y + _orientation.right * _stateMachine.CurrentMovementInput.x;
        inputY = _stateMachine.CurrentMovementInput.y;
        inputX = _stateMachine.CurrentMovementInput.x;

        inputDir.y = 0;

        currentCameraRotation = transform.rotation;
        float rotationChange = Quaternion.Angle(currentCameraRotation, previousCameraRotation);

        if (_stateMachine.IsAirborneState && rotationChange > _rotationThreshold)
        {
            Quaternion lookRotation = Quaternion.LookRotation(viewDir, Vector3.up);
            _playerObj.transform.rotation = Quaternion.Slerp(_playerObj.transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            previousCameraRotation = currentCameraRotation;
        }
        else if (inputDir != Vector3.zero && !_stateMachine.IsAirborneState)
        {


            if (inputY == -oldInputY && inputY == 1f || inputY == -oldInputY && inputY == -1f || inputX == -oldInputX && inputX == 1f || inputX == -oldInputX && inputX == -1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(inputDir, Vector3.up);
                _playerObj.transform.rotation = lookRotation;
            }
            else
            {
                Quaternion lookRotation = Quaternion.LookRotation(inputDir, Vector3.up);
                _playerObj.transform.rotation = Quaternion.Slerp(_playerObj.transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            }

        }

        if (inputY != 0)
        {
            oldInputY = inputY;
            oldInputX = inputX;
        }
        if (inputX != 0)
        {
            oldInputX = inputX;
            oldInputY = inputY;
        }
    }
}
