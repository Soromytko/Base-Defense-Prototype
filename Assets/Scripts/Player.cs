using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    public float CurrentSpeed { get; private set; }

    [SerializeField] private float _runSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _animationSpeed = 1f;
    [SerializeField] private Joystick _joystick;

    private Rigidbody _rigidBody;
    private CapsuleCollider _capsuleCollider;
    private Animator _animator;
    private CameraSystem _cameraSystem;

    private bool isRun = false;
    private Vector3 _lastPosition;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();

        //if ((_cameraSystem = FindObjectOfType<CameraSystem>()) == null)
        //    throw new System.Exception("CameraSystem is not founded");

        _cameraSystem = FindObjectOfType<CameraSystem>() ??
            throw new System.Exception("CameraSystem is not founded");

    }

    private void FixedUpdate()
    {
        isRun = false;

        Vector3 moveDirection = _cameraSystem.Camera.transform.TransformDirection(new Vector3(_joystick.Horizontal, 0, _joystick.Vertical));
        Vector3 velocity = _rigidBody.velocity;

        velocity.x = moveDirection.x * _runSpeed;
        velocity.z = moveDirection.z * _runSpeed;
        _rigidBody.velocity = velocity;

        _cameraSystem.Pursue(transform.position);

        if (_joystick.Direction != Vector2.zero)
        {
            Quaternion rotationTarget = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, Time.fixedDeltaTime * _rotationSpeed);
            isRun = true;
        }

        _animator.SetBool("IsRun", isRun);
        _animator.SetFloat("AnimSpeed", _animationSpeed * moveDirection.magnitude);
    }

}
