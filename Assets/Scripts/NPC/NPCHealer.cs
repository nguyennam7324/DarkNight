using UnityEngine;
using TMPro;

public class NPCHealer : MonoBehaviour
{
    [Header("Thiết lập NPC")]
    [SerializeField] private TMP_Text chatText; // gắn Text (TMP) ở ChatCanvas
    [SerializeField] private GameObject potionPrefab; // prefab health potion
    [SerializeField] private Transform dropPoint; // chỗ thả potion

    [Header("Cooldown")]
    [SerializeField] private float interactCooldown = 10f; // thời gian hồi tương tác
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
                chatText.text = "Bạn có muốn hồi máu không? (E để đồng ý / Z để bỏ)";
                chatText.gameObject.SetActive(true);
                isInteracting = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                DropPotion();
                EndInteraction();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                EndInteraction();
            }
        }
    }

    private void DropPotion()
    {
        if (potionPrefab != null && dropPoint != null)
        {
            Instantiate(potionPrefab, dropPoint.position, Quaternion.identity);
            Debug.Log("💊 NPC thả potion!");
        }
        nextInteractTime = Time.time + interactCooldown; // bắt đầu cooldown
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
