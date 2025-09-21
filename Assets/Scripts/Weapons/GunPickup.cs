using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public InventoryItemSO gunItem; // Tham chiếu đến InventoryItemSO của súng
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GunHolder gunHolder = collision.GetComponentInChildren<GunHolder>();
            if (gunHolder != null)
            {
                if (gunHolder.gameObject.transform.childCount > 0)
                {
                    // Đã có súng, bỏ vào rương
                    InventoryManager.Instance.AddItem(gunItem);
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    gunHolder.EquipGun(this.gameObject);
                    return; // Không destroy ở đây để GunHolder xử lý
                }
            }
        }
    }
    void OnEnable() { Debug.Log($"{name} được ENABLE bởi: {GetCaller()}"); }
    void OnDisable() { Debug.Log($"{name} được DISABLE bởi: {GetCaller()}"); }

    private string GetCaller()
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        if (stackTrace.FrameCount > 2)
        {
            return stackTrace.GetFrame(2).GetMethod().Name;
        }
        return "Unknown";
    }
}