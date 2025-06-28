using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float duration = 5f;
    private bool collected = false;
    private AudioSource audioSource;

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
                collected = true;
                player.ApplySpeedBoost(speedMultiplier, duration);

                // Ẩn item (sprite + collider)
                foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
                    sr.enabled = false;
                Collider2D col = GetComponent<Collider2D>();
                if (col) col.enabled = false;

                // Phát âm thanh nếu có
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.Play();
                    Destroy(gameObject, audioSource.clip.length);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
