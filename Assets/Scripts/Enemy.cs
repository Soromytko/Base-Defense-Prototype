using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Enemy : MonoBehaviour, IAttack, IDie
{
    public float Health { get => _health; set { _health = value; /*_healthBar.value = value;*/ } }

    [SerializeField] private float _health = 100f;
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _attackRadius = 0.2f;
    [SerializeField] private Transform _weapon;
    //[SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject[] _bonuses;

    private void Update()
    {
        //_healthBar.Position = transform.position + Vector3.up * 2;

        if (Input.GetKeyDown(KeyCode.G))
            TakeDamage(10);

        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(FindObjectOfType<Player>().transform.position - transform.position);
    }

    public void TakeDamage(float damage)
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

        Destroy(gameObject);
    }



    private void OnDrawGizmos()
    {
        if (!_weapon) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_weapon.position, _attackRadius);
    }

}
