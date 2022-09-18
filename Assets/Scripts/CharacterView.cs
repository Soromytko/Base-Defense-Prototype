using UnityEngine;

public abstract class CharacterView : MonoBehaviour
{
    [SerializeField] protected HealthBar _healthBarPrefab;
    [SerializeField] protected Transform _healthBarPoint;
    protected HealthBar _healthBar;
    protected Character _character;

    protected void OnHealthChanged(float newValue) => _healthBar.Value = newValue;

    private void OnEnable()
    {
        _healthBar = Instantiate(_healthBarPrefab, FindObjectOfType<Canvas>().transform);
        _healthBar.Target = _healthBarPoint;
        _character.HealthHandler += OnHealthChanged;
    }

    private void OnDisable()
    {
        _character.HealthHandler -= OnHealthChanged;
        if(_healthBar != null) Destroy(_healthBar.gameObject);
    }

}