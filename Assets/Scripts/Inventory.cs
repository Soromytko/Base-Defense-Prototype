using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class Inventory : MonoBehaviour
{
    public int BonusAmount { get => _bonusAmount; set { _bonusAmount = value; _text.text = _bonusAmount.ToString(); } }
    [SerializeField] private int _bonusAmount;
    [SerializeField] private Text _text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Bonus>(out Bonus bonus))
        {
            BonusAmount += bonus.Cost;
            Destroy(bonus.gameObject);
        }
        else if(other.transform.TryGetComponent<BaseInventory>(out BaseInventory baseInventory))
        {
            baseInventory.BonusAmount += BonusAmount;
            BonusAmount = 0;
        }
    }

}
