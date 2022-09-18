using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyView : CharacterView
{
    private void Awake()
    {
        _character = GetComponent<Enemy>();
    }

}
