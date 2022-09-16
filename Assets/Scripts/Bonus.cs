using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Bonus : MonoBehaviour
{
    public event Action<Bonus> TakeHandler;
    public int Cost => _cost;
    [SerializeField] private int _cost;
}
