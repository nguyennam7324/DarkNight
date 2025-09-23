using System;
using TMPro;
using UnityEngine;

public class WinGameController : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI deathText;
    internal void PopulateData(int level, int numberDeath)
    {
        levelText.text = "Level: " + level;
        deathText.text = "Số quái đã diệt: " + numberDeath;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Reset()
    {
        GameManager.instance.level = 0;
        GameManager.instance.numberDeath = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
