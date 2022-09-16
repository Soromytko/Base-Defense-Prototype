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

        _cameraSystem = FindObjectOfType<CameraSystem>() ??
            throw new System.Exception("CameraSystem is not founded");
    }

    private void FixedUpdate()
    {
        CurrentSpeed = (_rigidBody.position - _lastPosition).magnitude;
        _lastPosition = _rigidBody.position;

        Vector3 moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

#if UNITY_STANDALONE_WIN
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
#endif

        moveDirection = _cameraSystem.Camera.transform.TransformDirection(moveDirection);

        isRun = moveDirection != Vector3.zero;

        Vector3 velocity = _rigidBody.velocity;

        velocity.x = moveDirection.x * _runSpeed;
        velocity.z = moveDirection.z * _runSpeed;
        _rigidBody.velocity = velocity;

        _cameraSystem.Pursue(transform.position);

        if (isRun)
        {
            Quaternion rotationTarget = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, Time.fixedDeltaTime * _rotationSpeed);
            isRun = true;
        }

        _animator.SetBool("IsRun", isRun);
        _animator.SetFloat("AnimSpeed", _animationSpeed * moveDirection.magnitude);
    }

}
