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
        if (playerInRange)
        {
            if (Time.time >= nextInteractTime)
            {
                if (!isInteracting)
                {
                    chatText.text = "Nhấn E để hỏi?";
                    chatText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        isInteracting = true;
                        chatText.text = "Bạn có muốn hồi máu không? (E để đồng ý / Z để bỏ)";
                    }
                }
                else
                {
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
            else
            {
                // hiển thị thời gian còn lại
                float remain = Mathf.Ceil(nextInteractTime - Time.time);
                chatText.text = $"⏳ Chờ {remain}s để có thể hồi máu tiếp";
                chatText.gameObject.SetActive(true);
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
