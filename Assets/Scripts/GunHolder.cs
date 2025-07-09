using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [Header("Điểm gắn súng (nên đặt ở tay)")]
    public Transform gunHoldPoint;

    private GameObject currentGun;

    public void EquipGun(GameObject newGun)
    {
        // Xóa súng cũ nếu có
        if (currentGun != null)
        {
            Destroy(currentGun);
        }

        // Tạo bản sao súng và gắn vào điểm cầm
        currentGun = Instantiate(newGun, gunHoldPoint.position, Quaternion.identity, gunHoldPoint.transform);
        currentGun.SetActive(true);


        // Xoá súng dưới đất
        Destroy(newGun);
    }
}
