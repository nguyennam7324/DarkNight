using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public float healAmount = 50f;
    public InventoryItemSO itemSO;
    private AudioSource audioSource;
    private bool collected = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("TRIGGER ENTER");

            InventoryManager.Instance.AddItem(itemSO);
        }
    }
}
