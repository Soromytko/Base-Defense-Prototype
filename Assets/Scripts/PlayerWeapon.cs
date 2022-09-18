using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _fireTimeout = 0.5f;
    [SerializeField] private Bullet _bullet;

    private float fireTimeout;

    private void FixedUpdate()
    {
        if (fireTimeout < _fireTimeout)
        {
            fireTimeout += Time.deltaTime;
            return;
        }

        fireTimeout = 0;

        Collider[] enemies = Physics.OverlapSphere(transform.position, _radius, LayerMask.GetMask("Enemy"));
        if (enemies.Length == 0) return;

        float minDistance = int.MaxValue;
        Enemy resultEnemy = null;
        foreach (var enemy in enemies)
        {
            float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            Enemy currentEnemy = enemy.GetComponent<Enemy>();
            if (currentDistance < minDistance && currentEnemy.Health > 0)
            {
                minDistance = currentDistance;
                resultEnemy = currentEnemy;
            }
        }

        if (resultEnemy == null) return;

        var fireDirection = (resultEnemy.transform.position - transform.position);
        Fire(new Vector3(fireDirection.x, 0, fireDirection.z).normalized);
    }

    private void Fire(Vector3 direction)
    {
        var bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        bullet.Direction = direction;
    }

}
