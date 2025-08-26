using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
public class itemCollector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    
    [SerializeField] private int level;
    public float CurrentXp;
    [SerializeField] private float targetXp;
    [SerializeField] private Image xpProgressBar;
    public int gold = 0;
    public UpgradeUI upgradeUI;
    public float xpMultiplier = 1f; // ✨ thêm multiplier mặc định = 100%
    void Start()
    {
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gold"))
        {
            gold++;
        
            Destroy(collision.gameObject);
            Debug.Log("Gold: " + gold);
        }
        if (collision.gameObject.CompareTag("Silver"))
        {
            gold++;
          
            Destroy(collision.gameObject);
            float gainedXp = 12 * xpMultiplier; // ✨ tính theo multiplier
            CurrentXp += gainedXp;
            Debug.Log($"Nhận được {gainedXp} XP (x{xpMultiplier})");
        }
        experienceText.text = CurrentXp + " / " + targetXp;
        experienceController();
    }
    public void experienceController()
    {
        levelText.text = "Level : " + level.ToString();
        xpProgressBar.fillAmount = (CurrentXp / targetXp);
        if (CurrentXp >= targetXp) // ✨ dùng >= thay vì ==
        {
            CurrentXp = 0;
            level++;
            targetXp *= 1.2f;
            Time.timeScale = 0;
            upgradeUI.ShowUpgradeOptions();
        }
    }
}


