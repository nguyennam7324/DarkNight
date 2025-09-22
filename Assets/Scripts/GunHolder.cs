using UnityEngine;
using TMPro;
using System.Collections;

public class GunHolder : MonoBehaviour
{
    [Header("Điểm gắn súng (nên đặt ở tay)")]
    public Transform gunHoldPoint;

    [Header("UI hiển thị đạn")]
    [SerializeField] private TextMeshProUGUI ammoText; // Kéo text UI từ Canvas vào đây

    private GameObject currentGun;

    public void EquipGun(GameObject pickupGun)
    {
        if (pickupGun == null) return;

        // 1. Nếu đang có súng → drop
        if (currentGun != null)
        {
            DropCurrentGun();
        }

        // 2. Gắn súng mới vào tay
        pickupGun.transform.SetParent(gunHoldPoint);
        pickupGun.transform.localPosition = Vector3.zero;
        pickupGun.transform.localRotation = Quaternion.identity;
        pickupGun.transform.localScale = Vector3.one;

        pickupGun.SetActive(true);
        currentGun = pickupGun;

        // 3. Thiết lập biến cho súng
        IGun gunScript = currentGun.GetComponent<IGun>();
        if (gunScript != null)
        {
            gunScript.SetAmmoText(ammoText);  // Gán UI ammo
            gunScript.SetEquipped(true);
            gunScript.SetAudioManager(FindObjectOfType<AudioManager>());
        }

        // 4. Xoá pickup nếu khác instance
        if (pickupGun != currentGun)
        {
            Destroy(pickupGun);
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
        {
            gunScript.SetEquipped(false);
            gunScript.SetAmmoText(null); // Clear UI ammo
        }

        // Khi drop súng → reset text về EMPTY
        if (ammoText != null)
            ammoText.text = "EMPTY";

        currentGun = null;
    }
}
