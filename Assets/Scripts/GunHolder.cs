using UnityEngine;
using TMPro;

public class GunHolder : MonoBehaviour
{
    [Header("Điểm gắn súng (tay Player)")]
    public Transform gunHoldPoint;

    [Header("UI hiển thị đạn")]
    [SerializeField] private TextMeshProUGUI ammoText;

    private GameObject currentGun;

    public void EquipGun(GameObject pickupGun)
    {
        // Drop súng cũ nếu có
        if (currentGun != null)
        {
            DropCurrentGun();
        }

        // Gắn súng mới vào tay
        pickupGun.transform.SetParent(gunHoldPoint);
        pickupGun.transform.localPosition = Vector3.zero;
        pickupGun.transform.localRotation = Quaternion.identity;
        pickupGun.transform.localScale = Vector3.one;

        pickupGun.SetActive(true);
        currentGun = pickupGun;

        // Thiết lập IGun
        IGun gunScript = currentGun.GetComponent<IGun>();
        if (gunScript != null)
        {
            gunScript.SetEquipped(true);
            gunScript.SetAmmoText(ammoText); // 👉 Truyền đúng ammoText
            gunScript.SetAudioManager(FindObjectOfType<AudioManager>());
        }
    }

    private void DropCurrentGun()
    {
        if (currentGun == null) return;

        currentGun.transform.SetParent(null);

        Collider2D col = currentGun.GetComponent<Collider2D>();
        if (col) col.enabled = true;

        Rigidbody2D rb = currentGun.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(Random.Range(-1f, 1f), 1f) * 3f, ForceMode2D.Impulse);
        }

        GunPickup pickup = currentGun.GetComponent<GunPickup>();
        if (pickup != null)
            pickup.enabled = true;

        IGun gunScript = currentGun.GetComponent<IGun>();
        if (gunScript != null)
            gunScript.SetEquipped(false);

        currentGun = null;
    }
}
