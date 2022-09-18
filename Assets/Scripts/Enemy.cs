using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : Character, IAttack, IDie
{
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _weaponRadius = 0.2f;
    [SerializeField] private float _attackDistance = 1f;
    [SerializeField] private Transform _weapon;
    [SerializeField] private GameObject[] _bonuses;

    private Animator _animator;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;

        if (other.TryGetComponent<Player>(out Player player))
        {
            if (player.Safe)
            {
                _agent.SetDestination(transform.position);
                _animator.Play("Idle");
            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= _attackDistance)
                {
                    _animator.Play("Attack");
                }
                else
                {
                    _agent.SetDestination(player.transform.position);
                    _animator.Play("Walk");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            _agent.SetDestination(transform.position);
            _animator.Play("Idle");
        }
    }

    public override void TakeDamage(float damage)
    {
        Health -= damage;

        if (_health <= 0)
        {
            _agent.enabled = false;
            enabled = false;
            GetComponent<Animator>().Play("Die");
        }
    }

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(_weapon.position, _weaponRadius, LayerMask.GetMask("Player"));

        foreach (var item in colliders)
            if (item.TryGetComponent<Player>(out Player player))
                player.TakeDamage(_damage);
    }

    public void Die()
    {
        foreach (var bonus in _bonuses)
            Instantiate(bonus, transform.position, Quaternion.identity);

        _dieHandler?.Invoke();
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        if (!_weapon) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_weapon.position, _weaponRadius);
    }

}
