using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventorySlot> InventorySlots = new List<InventorySlot>();
    public List<GameObject> guns = new List<GameObject>();
    public int maxSlots = 20;
    public GameObject inventoryBoard;
    public GameObject itemPrefab;

    


    int selectedSlot = -1;

    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
        {
            InventorySlots[newValue].Deselect();
        }
        InventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InventorySlots.AddRange(gameObject.GetComponentsInChildren<InventorySlot>());
        inventoryBoard.SetActive(false);
        selectedSlot = 0;
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number >= 0 && number < 8)
            {
                ChangeSelectedSlot(number - 1);

            }
        }

    }

    public void OpenOrHideInventoryBoard(GameObject inventoryButton)
    {
        if(inventoryBoard.activeSelf)
        {
            inventoryButton.SetActive(true);
            inventoryBoard.SetActive(false);
        }
        else
        {
            inventoryButton.SetActive(false);
            inventoryBoard.SetActive(true);
        }
    }

    public void SpawnItem(InventoryItemSO item, InventorySlot slot)
    {
        GameObject g = Instantiate(itemPrefab, slot.transform);
        InventoryItem inventoryItem = g.GetComponent<InventoryItem>();
        inventoryItem.InitItem(item);
    } 

    public bool AddItem(InventoryItemSO item)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
            if (inventoryItem!=null && inventoryItem.item == item)
            {
                inventoryItem.amount++;
                inventoryItem.RefreshAmount();
                return true;
            }
        }

        for (int i = 0; i < InventorySlots.Count; i++)
        {
            InventorySlot slot = InventorySlots[i];
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
            if(inventoryItem == null)
            {
                SpawnItem(item, slot);
                return true;
            }     
        }
        return false;
    }


    public InventoryItemSO GetSelectedItem(bool use)
    {
        InventorySlot slot = InventorySlots[selectedSlot];
        InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
        if (inventoryItem != null)
        {
            InventoryItemSO itemSO = inventoryItem.item;
            if (use)
            {
               UsingItem(inventoryItem);  
            }
            return itemSO;
        }
        return null;
    }

    public void UsingItem(InventoryItem inventoryItem)
    {
        inventoryItem.amount--;
        if (inventoryItem.amount <= 0)
        {
            Destroy(inventoryItem.gameObject);
        }
        inventoryItem.RefreshAmount();
        Player.instance.UseItem(inventoryItem.item);
    }   
}
//SO, Manager
//inventory behaviour
//miniBoss