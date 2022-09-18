using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Direction { get; set; }

    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _lifetime = 5;

    private int _mask;
    private Vector3 _prevPosition;

    private void Awake()
    {
        _mask = ~(LayerMask.GetMask("Enemy") | LayerMask.GetMask("Defautl"));
        _mask = LayerMask.GetMask("Enemy");
        _prevPosition = transform.position;
        Destroy(gameObject, _lifetime);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Direction, out hit, Vector3.Distance(transform.position, _prevPosition), _mask))
        {
            var enemy = hit.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }

        _prevPosition = transform.position;
        transform.position += Direction * _speed * Time.fixedDeltaTime;
    }
}
