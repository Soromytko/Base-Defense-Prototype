using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class BaseInventory : MonoBehaviour
{
    public int BonusAmount { get => _bonusAmount; set { _bonusAmount = value; _text.text = _bonusAmount.ToString(); } }
    [SerializeField] private int _bonusAmount;
    [SerializeField] private Text _text;
}
