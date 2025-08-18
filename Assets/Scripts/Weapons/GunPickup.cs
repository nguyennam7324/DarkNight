using UnityEngine;

public class GunPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GunHolder gunHolder = collision.GetComponentInChildren<GunHolder>();
            if (gunHolder != null)
            {
                gunHolder.EquipGun(this.gameObject);
                return; // Không destroy ở đây để GunHolder xử lý
            }
        }
    }
}