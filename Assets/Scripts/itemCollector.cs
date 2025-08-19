using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    private int gold = 0;
    private IGun currentGun;
    private ManaSystem manaSystem;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI goldText; // hiển thị vàng trên UI

    private void Start()
    {
        currentGun = GetComponentInChildren<IGun>();
        manaSystem = ManaSystem.instance;

        UpdateGoldUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nhặt vàng
        if (collision.CompareTag("Gold"))
        {
            Destroy(collision.gameObject);
            gold++;
            Debug.Log("💰 Gold: " + gold);
            UpdateGoldUI();
        }

        // Nhặt bạc → hồi Mana
        if (collision.CompareTag("Silver"))
        {
            Destroy(collision.gameObject);
            manaSystem?.AddMana(20); // hồi 20 mana
            Debug.Log("🔮 Hồi 20 Mana");
        }
    }
    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold.ToString();
        }
    }
}
