using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    public float ammoAmount = 8f;
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
            Gun gun = other.GetComponentInChildren<Gun>(); // Tìm trong con của player
            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
                collected = true;

                // Ẩn hình & collider
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
