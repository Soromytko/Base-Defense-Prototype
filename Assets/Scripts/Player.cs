using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour, IDie
{
    public float CurrentSpeed { get; private set; }

    public event Action<float> HealthHandler;
    public event Action DieHeandler;

    public float Health { get => _health; private set { _health = value;  HealthHandler?.Invoke(value); } }

    [SerializeField] private float _health = 100;
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
            throw new System.Exception("CameraSystem is not found");
    }

    private void FixedUpdate()
    {
        CurrentSpeed = (_rigidBody.position - _lastPosition).magnitude;
        _lastPosition = _rigidBody.position;

        Vector3 moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

#if UNITY_EDITOR
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
#endif

        moveDirection = _cameraSystem.Camera.transform.TransformDirection(moveDirection);

        isRun = moveDirection != Vector3.zero;

        _rigidBody.velocity = new Vector3(moveDirection.x * _runSpeed, _rigidBody.velocity.y, moveDirection.z * _runSpeed);
        _rigidBody.angularVelocity = Vector3.zero;

        _cameraSystem.Pursue(transform.position);

        if (isRun)
        {
            Quaternion rotationTarget = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            _rigidBody.rotation = Quaternion.Lerp(_rigidBody.rotation, rotationTarget, Time.fixedDeltaTime * _rotationSpeed);
        }

        _animator.SetBool("IsRun", isRun);
        _animator.SetFloat("AnimSpeed", _animationSpeed * moveDirection.magnitude);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (_health <= 0)
        {
            DieHeandler?.Invoke();



            enabled = false;
            _animator.Play("Die");
        }
    }

    public void Die()
    {
        DieHeandler?.Invoke();
        Destroy(gameObject);
    }

}
