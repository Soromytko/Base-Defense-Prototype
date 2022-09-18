using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemiesController : MonoBehaviour
{
    [SerializeField] private int _enemyMaxNumber = 10;
    [SerializeField] private int _currentEnemyCount = 0;
    [SerializeField] private float _spawnTimeout = 3f;
    [SerializeField] private Enemy _enemyPrefab;    

    private BoxCollider _boxCollider;
    private float _spawnTimer;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _spawnTimer = _spawnTimeout;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>() == null) return;

        if (_spawnTimer >= _spawnTimeout)
        {
            if (_currentEnemyCount < _enemyMaxNumber)
            {
                _currentEnemyCount++;
                SpawningEnemy();
                _spawnTimer = 0;
            }
        }
        else
        {
            _spawnTimer += Time.deltaTime;
        }
      
    }

    private void OnEnemyDie()
    {
        _currentEnemyCount--;
    }

    private void SpawningEnemy()
    {
        var enemy = Instantiate(_enemyPrefab, GetRandomPosition(), Quaternion.identity);
        enemy.DieHandler += OnEnemyDie;
        print("Spawn");
        //yield return null;
    }

    private Vector3 GetRandomPosition()
    {
        var min = transform.position + _boxCollider.center - _boxCollider.size / 2;
        var max = transform.position + _boxCollider.center + _boxCollider.size / 2;

        return new Vector3(Random.Range(min.x, max.x), max.y, Random.Range(min.z, max.z));
    }
}
