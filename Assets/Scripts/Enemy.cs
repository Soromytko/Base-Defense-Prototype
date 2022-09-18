using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Enemy : Character, IAttack, IDie
{
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _attackRadius = 0.2f;
    [SerializeField] private Transform _weapon;
    [SerializeField] private GameObject[] _bonuses;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            TakeDamage(10);

        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(FindObjectOfType<Player>().transform.position - transform.position);
    }

        public override void TakeDamage(float damage)
        {
            Health -= damage;

            if(_health <= 0)
            {
                GetComponent<Animator>().Play("Die");
            }
        }

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(_weapon.position, _attackRadius, LayerMask.GetMask("Player"));
        
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
        Gizmos.DrawWireSphere(_weapon.position, _attackRadius);
    }

}
