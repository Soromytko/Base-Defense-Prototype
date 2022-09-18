using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerView : CharacterView
{
    private void Awake()
    {
        _character = GetComponent<Player>();
    }

}
