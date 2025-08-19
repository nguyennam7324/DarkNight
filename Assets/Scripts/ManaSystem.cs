using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    public static ManaSystem instance;

    [Header("Mana Settings")]
    public float maxMana = 100f;
    public float currentMana;

    [Header("Regen Settings")]
    public float manaRegenPerSecond = 5f;

    [Header("UI")]
    [SerializeField] private Image manaFillImage; // 🔥 dùng Image thay vì Slider

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentMana = maxMana;
        UpdateUI();
    }

    void Update()
    {
        RegenMana();
    }

    void RegenMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenPerSecond * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, maxMana);
            UpdateUI();
        }
    }

    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddMana(float amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (manaFillImage != null)
            manaFillImage.fillAmount = currentMana / maxMana; // 🎯 cập nhật fill
    }
}
