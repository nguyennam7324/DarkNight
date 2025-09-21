using UnityEngine;
using TMPro;
using System.Collections;

public class GunHolder : MonoBehaviour
{
    [Header("Điểm gắn súng (nên đặt ở tay)")]
    public Transform gunHoldPoint;

    private GameObject currentGun;

    public void EquipGun(GameObject pickupGun)
    {

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


        // 3. Vô hiệu hóa Collider2D và Rigidbody2D
        Collider2D col = currentGun.GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Rigidbody2D rb = currentGun.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // 4. Vô hiệu hóa GunPickup script để không bị nhặt lại
        GunPickup pickupScript = currentGun.GetComponent<GunPickup>();
        if (pickupScript != null)
            pickupScript.enabled = false;

        // 5. Thiết lập biến
        IGun gunScript = currentGun.GetComponent<IGun>();
        if (gunScript != null)
        {
            gunScript.SetEquipped(true);
            gunScript.SetAmmoText(FindObjectOfType<TextMeshProUGUI>());
            gunScript.SetAudioManager(FindObjectOfType<AudioManager>());
        }
    }

    private void DropCurrentGun()
    {
        if (currentGun == null) return;

        currentGun.transform.SetParent(null);

        // Kích hoạt lại Collider2D
        Collider2D col = currentGun.GetComponent<Collider2D>();
        if (col) col.enabled = true;

        // Kích hoạt lại Rigidbody2D
        Rigidbody2D rb = currentGun.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(Random.Range(-1f, 1f), 1f) * 3f, ForceMode2D.Impulse);
        }

        // Kích hoạt lại GunPickup script
        GunPickup pickup = currentGun.GetComponent<GunPickup>();
        if (pickup != null)
            pickup.enabled = true;

        IGun gunScript = currentGun.GetComponent<IGun>();
        if (gunScript != null)
            gunScript.SetEquipped(false);

        currentGun = null;
    }
}
