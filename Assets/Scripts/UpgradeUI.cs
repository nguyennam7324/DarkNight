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
        public Sprite icon;   // 🖼 icon của buff
        public System.Action applyEffect;
    }

    public GameObject panel;
    public Button[] upgradeButtons;
    public TextMeshProUGUI[] upgradeTexts;
    public UpgradePopup upgradePopup;
    public Image[] upgradeIcons;

    [Header("Optional Default Icon")]
    public Sprite defaultIcon;   // icon fallback khi không load được

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
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                GameObject.FindWithTag("Player").GetComponent<Player>().baseDamage += 5f;
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Tăng Speed",
            description = "+10% Speed",
            rarity = Rarity.Common,
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                GameObject.FindWithTag("Player").GetComponent<Player>().ApplySpeedBoost(1.1f, 5f);
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Tăng Crit",
            description = "+10% Tỉ lệ chí mạng",
            rarity = Rarity.Rare,
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                GameObject.FindWithTag("Player").GetComponent<Player>().critChance += 0.1f;
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Hồi máu",
            description = "Hồi 50% máu",
            rarity = Rarity.Rare,
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.Heal(player.maxHp * 0.5f);
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Tăng Max HP",
            description = "+25 HP tối đa",
            rarity = Rarity.Epic,
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.maxHp += 25f;
                player.currentHP += 25f;
                Debug.Log("Tăng giới hạn HP thêm 25!");
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Hồi máu",
            description = "Hồi 10% máu",
            rarity = Rarity.Common,
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.Heal(player.maxHp * 0.1f);
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Tăng Max HP",
            description = "+10 HP tối đa",
            rarity = Rarity.Rare,
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.maxHp += 10f;
                player.currentHP += 10f;
                Debug.Log("Tăng giới hạn HP thêm 10!");
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Khiên Năng Lượng",
            description = "Tăng 50 khiên hồi phục tự động",
            rarity = Rarity.Rare,
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.maxShield += 50f;
                player.shield = player.maxShield;
            }
        });

        allUpgrades.Add(new Upgrade {
            upgradeName = "Hút Máu",
            description = "Hồi 10% máu theo sát thương gây ra",
            rarity = Rarity.Rare,
            icon = Resources.Load<Sprite>("Assets/Violet Theme Ui/Colored Icons/Sword_0.png"),
            applyEffect = () => {
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.lifeSteal += 0.1f;
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

            // text
            string colorHex = ColorUtility.ToHtmlStringRGB(GetColorByRarity(chosen.rarity));
            upgradeTexts[i].text = $"<color=#{colorHex}>{chosen.upgradeName}</color>\n{chosen.description}";
            upgradeTexts[i].color = GetColorByRarity(chosen.rarity);

            // icon
            if (upgradeIcons.Length > i && upgradeIcons[i] != null)
            {
                if (chosen.icon != null)
                {
                    upgradeIcons[i].sprite = chosen.icon;
                    upgradeIcons[i].preserveAspect = true;
                }
                else
                {
                    Debug.LogWarning($"⚠️ Upgrade '{chosen.upgradeName}' chưa có icon! Dùng icon mặc định.");
                    if (defaultIcon != null)
                    {
                        upgradeIcons[i].sprite = defaultIcon;
                        upgradeIcons[i].preserveAspect = true;
                    }
                }
            }
            else
            {
                Debug.LogError($"❌ Không tìm thấy upgradeIcons[{i}] trong Inspector!");
            }

            // click
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(chosen));
        }
    }

    void SelectUpgrade(Upgrade upgrade)
    {
        Debug.Log("Selected upgrade: " + upgrade.upgradeName);

        upgrade.applyEffect?.Invoke();

        if (upgradePopup != null)
        {
            string msg = upgrade.upgradeName;
            upgradePopup.Show(msg);
        }

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
