using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public float healAmount = 50f;
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
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Heal(healAmount);
                collected = true;

                // 1. Ẩn sprite và collider
                foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.enabled = false;
                }

                Collider2D col = GetComponent<Collider2D>();
                if (col) col.enabled = false;

                // 2. Phát âm thanh
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.Play();
                    Destroy(gameObject, audioSource.clip.length); // Hủy sau khi phát xong
                }
                else
                {
                    Destroy(gameObject); // Không có âm thì phá hủy ngay
                }
            }
        }
    }
}
