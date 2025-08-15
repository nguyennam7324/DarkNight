using TMPro;
using UnityEngine;

public class AutoShotgun : MonoBehaviour, IGun
{
    private float rotationOffSet = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float delayShot = 0.2f;
    [SerializeField] private float maxAmmo = 8f;
    public float currentAmmo;
    private float nextShot;

    [SerializeField] private TextMeshProUGUI ammoText;
    public bool isEquipped = false;

    [Header("Spread")]
    [SerializeField] private int pelletsPerShot = 8;
    [SerializeField] private float totalSpreadAngle = 25f;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.15f;
    [SerializeField] private float recoilReturnSpeed = 4f;
    private Vector3 originalLocalPos;

    [Header("Reload Delay")]
    [SerializeField] private float reloadDelay = 2f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        originalLocalPos = transform.localPosition;
        UpdateAmmoText();
    }

    void Update()
    {
        if (!isEquipped) return;

        HandleRecoil();

        if (isReloading || Time.time < nextShot) return;

        RotationGun();
        Shot();
    }

    void RotationGun()
    {
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffSet);
        transform.localScale = (angle < -90 || angle > 90) ? Vector3.one : new Vector3(1, -1, 1);
    }

    void Shot()
    {
        if (Input.GetMouseButton(0) && currentAmmo > 0)
        {
            nextShot = Time.time + delayShot;

            float startAngle = -totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (pelletsPerShot - 1);

            for (int i = 0; i < pelletsPerShot; i++)
            {
                float angleOffset = startAngle + angleStep * i;
                Quaternion pelletRotation = firePos.rotation * Quaternion.Euler(0, 0, angleOffset);
                Instantiate(bulletPrefab, firePos.position, pelletRotation);
            }

            currentAmmo--;
            UpdateAmmoText();

            // Recoil
            transform.localPosition -= transform.right * recoilDistance;

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    private System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDelay);
        currentAmmo = maxAmmo;
        UpdateAmmoText();
        isReloading = false;
    }

    void HandleRecoil()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPos, Time.deltaTime * recoilReturnSpeed);
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo > 0 ? currentAmmo.ToString() : "EMPTY";
    }

    public void AddAmmo(float amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        UpdateAmmoText();
    }
    public void SetAudioManager(AudioManager audioManager)
    {
        // Tạm bỏ trống vì chưa cần dùng audio chung
    }

    public void SetOriginalLocalPos()
    {
        originalLocalPos = transform.localPosition;
    }

    public void SetEquipped(bool equipped) => isEquipped = equipped;
    public void SetAmmoText(TextMeshProUGUI text) => ammoText = text;
}
