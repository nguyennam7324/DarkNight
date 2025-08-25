using UnityEngine;
using TMPro;

public class ShopNPC : MonoBehaviour
{
    [Header("Thiết lập NPC Shop")]
    [SerializeField] private TMP_Text chatText;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] gunPrefabs;

    [Header("Cooldown")]
    [SerializeField] private float interactCooldown = 60f;
    private float nextInteractTime = 0f;

    private bool playerInRange = false;
    private bool isInteracting = false;

    void Start()
    {
        chatText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Time.time >= nextInteractTime)
        {
            if (!isInteracting)
            {
                // chỉ nhắc bấm E
                chatText.text = "Nhấn E để hỏi?";
                chatText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    isInteracting = true;
                    ShowShopOptions();
                }
            }
            else
            {
                CheckInput();
            }
        }
        else if (playerInRange && Time.time < nextInteractTime)
        {
            float remain = Mathf.Ceil(nextInteractTime - Time.time);
            chatText.text = $"Shop sẽ mở lại sau {remain}s";
            chatText.gameObject.SetActive(true);
        }
    }

    private void ShowShopOptions()
    {
        string msg = "Bạn muốn khẩu súng gì?\n";
        for (int i = 0; i < gunPrefabs.Length; i++)
        {
            msg += $"{i + 1} - {gunPrefabs[i].name}\n";
        }
        msg += "ESC - Hủy";
        chatText.text = msg;
    }

    private void CheckInput()
    {
        for (int i = 0; i < gunPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SpawnGun(gunPrefabs[i]);
                EndInteraction();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndInteraction();
        }
    }

    private void SpawnGun(GameObject prefab)
    {
        if (spawnPoint != null && prefab != null)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            Debug.Log($"🔫 NPC Shop thả {prefab.name}");
        }
        nextInteractTime = Time.time + interactCooldown;
    }

    private void EndInteraction()
    {
        chatText.gameObject.SetActive(false);
        isInteracting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            chatText.text = "Nhấn E để hỏi?";
            chatText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            EndInteraction();
        }
    }
}
