using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    public enum Rarity
    {
        Common,
        Rare,
        Epic
    }

    [System.Serializable]
    public class Upgrade
    {
        public string upgradeName;
        public string description;
        public Rarity rarity;
        public System.Action applyEffect;
    }

    public GameObject panel;
    public Button[] upgradeButtons;
    public TextMeshProUGUI[] upgradeTexts;

    private List<Upgrade> allUpgrades = new List<Upgrade>();

    void Start()
    {
        panel.SetActive(false);
        LoadUpgrades();
    }

    void LoadUpgrades()
    {
        allUpgrades.Add(new Upgrade {
            upgradeName = "Tăng Damage",
            description = "+20% Damage",
            rarity = Rarity.Common,
            applyEffect = () => {
                Debug.Log("Damage +20%");
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Tăng Speed",
            description = "+10% Speed",
            rarity = Rarity.Common,
            applyEffect = () => {
                GameObject.FindWithTag("Player").GetComponent<Player>().ApplySpeedBoost(1.1f, 5f);
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Tăng Crit",
            description = "+10% Tỉ lệ chí mạng",
            rarity = Rarity.Rare,
            applyEffect = () => {
                GameObject.FindWithTag("Player").GetComponent<Player>().critChance += 0.1f;
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Hồi máu",
            description = "Hồi 50% máu",
            rarity = Rarity.Rare,
            applyEffect = () => {
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.Heal(player.maxHp * 0.5f);
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Tăng Max HP",
            description = "+25 HP",
            rarity = Rarity.Epic,
            applyEffect = () => {
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.Heal(25f);
                // Tuỳ chỉnh nếu muốn tăng luôn maxHP thật
            }
        });

        allUpgrades.Add(new Upgrade {
    upgradeName = "Tăng EXP",
    description = "+25% XP nhận được",
    rarity = Rarity.Rare,
    applyEffect = () => {
        GameObject.FindWithTag("Player")
            .GetComponent<itemCollector>().xpMultiplier += 0.25f;
        Debug.Log("Tăng EXP multiplier lên 25%");
    }
});
    }

    public void ShowUpgradeOptions()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;

        List<Upgrade> randomUpgrades = new List<Upgrade>(allUpgrades);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (randomUpgrades.Count == 0) break;

            int randomIndex = Random.Range(0, randomUpgrades.Count);
            Upgrade chosen = randomUpgrades[randomIndex];
            randomUpgrades.RemoveAt(randomIndex);

            string colorHex = ColorUtility.ToHtmlStringRGB(GetColorByRarity(chosen.rarity));
            upgradeTexts[i].text = $"<color=#{colorHex}>{chosen.upgradeName}</color>\n{chosen.description}";
            upgradeTexts[i].color = GetColorByRarity(chosen.rarity);

            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(chosen));
        }
    }

    void SelectUpgrade(Upgrade upgrade)
    {
        Debug.Log("Selected upgrade: " + upgrade.upgradeName);
        upgrade.applyEffect?.Invoke();
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    Color GetColorByRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.white;
            case Rarity.Rare: return new Color(0.3f, 0.8f, 1f);
            case Rarity.Epic: return new Color(0.7f, 0.3f, 1f);
            default: return Color.gray;
        }
    }
}