using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public event Action<float> HealthHandler { add { _healthHandler += value; }  remove { _healthHandler -= value; } }
    public event Action DieHandler { add { _dieHandler += value; } remove { _dieHandler -= value; } }
    public float Health { get => _health; set { _health = value; _healthHandler?.Invoke(value); } }

    protected Action<float> _healthHandler;
    protected Action _dieHandler;
    [SerializeField] protected float _health;

    public abstract void TakeDamage(float damage);

}
