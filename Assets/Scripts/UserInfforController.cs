using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfforController : MonoBehaviour
{
    public Image hp;
    public Image mp;
    public Image amor;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI amorText;

    public Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.didAwake)
        {
            hp.fillAmount = (float)player.currentHP / player.maxHp;
            mp.fillAmount = (float)player.currentMana / player.maxMana;
            amor.fillAmount = (float)player.shield / player.maxShield;
            hpText.text = player.currentHP + "/" + player.maxHp;
            mpText.text = player.currentMana + "/" + player.maxMana;
            amorText.text = player.shield + "/" + player.maxShield;
        }
    }
}
