using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float rotationOffSet = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float DelayShot = 0.15f;
    public float currentAmmo;
    [SerializeField] private float maxAmmo = 24f;
    private float nextShot;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] AudioManger audioManager;
    void Start()
    {
        currentAmmo = maxAmmo;
    }

   
    void Update()
    {
        RotationGun();
        Shot();
        Reload();
        UpdateAmmotext();
    }
    void RotationGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffSet);
        if (angle < -90 || angle > 90)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }

    void Shot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + DelayShot;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
            UpdateAmmotext();
            audioManager.playShotSound();
        }
    }
    void Reload()
    {
        if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
            UpdateAmmotext();
            audioManager.playReloadSound();
        }
    }
    public void AddAmmo(float amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        UpdateAmmotext();
    }
    void UpdateAmmotext()
    {
        if (ammoText != null)
        {
            if (currentAmmo > 0)
            {
                ammoText.text = currentAmmo.ToString();
            }
            else
            {
                ammoText.text = "EMPTY";
            }
        }
    }
}
