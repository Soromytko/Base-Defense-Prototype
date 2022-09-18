using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float Value { get => _fill.sizeDelta.x; set { _fill.sizeDelta = new Vector2(value, _fill.sizeDelta.y); } }
    public Transform Target { get; set; }
    [SerializeField] private RectTransform _fill;

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(Target.position);
    }

}
