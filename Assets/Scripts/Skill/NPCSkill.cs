using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSkill : MonoBehaviour
{
    public static NPCSkill instance;
    public List<SkillItem> skillItems;
    public List<SkillItemSO> skillItemSO;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowSkill();
    }

    private void ShowSkill()
    {
        ShuffleList(skillItemSO);
        for (int i = 0; i < skillItems.Count; i++)
        {
            skillItems[i].SetSkill(skillItemSO[i]);
        }
    }

    // Update is called once per frame
    public void Reset()
    {
        for (int i = 0; i < skillItems.Count; i++)
        {
            skillItems[i].gameObject.SetActive(true);
        }
    }

    public static void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void Show()
    {
        var player = GameObject.FindWithTag("Player");
        gameObject.transform.position = player.transform.position + new Vector3(0, 2.5f,0);
        gameObject.SetActive(true);
        Reset();
        ShowSkill();
    }

    internal void SelectedSkill(SkillItem skillItem)
    {
        foreach (var item in skillItems)
        {
            if (item != skillItem)
            {
                item.gameObject.SetActive(false);
            }
        }
        gameObject.SetActive(false);
        DoubleDoorController.instance.OpenNextDoor();
    }
}
