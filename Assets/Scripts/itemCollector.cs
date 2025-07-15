using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class itemCollector : MonoBehaviour

{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private int level;
    public float CurrentXp;
    [SerializeField] private float targetXp;
    [SerializeField] private Image xpProgressBar;
    private int gold = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gold"))
        {
            Destroy(collision.gameObject);
            gold++;
            Debug.Log("Gold: " + gold);
        }
        if (collision.gameObject.CompareTag("Silver"))
        {
            Destroy(collision.gameObject);
            CurrentXp += 12;
        }
        experienceText.text = CurrentXp + " / " + targetXp;
        experienceController();
    }
    public void experienceController()
    {
        levelText.text = "Level : " + level.ToString();
        xpProgressBar.fillAmount = (CurrentXp / targetXp);
        if (CurrentXp >= targetXp)
        {
            CurrentXp = CurrentXp - targetXp;
            level++;
            targetXp += 50;
        }
    }
}
