using TMPro;
using UnityEngine;

public class Shotgun : MonoBehaviour, IGun
{
    private float rotationOffSet = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float DelayShot = 1.0f;
    [SerializeField] private float maxAmmo = 8f;
    public float currentAmmo;
    private float nextShot;

    [SerializeField] public TextMeshProUGUI ammoText;
    [SerializeField] public AudioManager audioManager;
    public bool isEquipped = false;

    [Header("Spread")]
    [SerializeField] private int pelletsPerShot = 8;
    [SerializeField] private float totalSpreadAngle = 25f;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.15f;
    [SerializeField] private float recoilReturnSpeed = 4f;

    [Header("Reload Delay")]
    [SerializeField] private float fullReloadDelay = 2f;
    private bool isReloading = false;
    private Vector3 originalLocalPos;


    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (!isEquipped) return;

        HandleRecoil();

        if (isReloading || Time.time < nextShot) return;

        RotationGun();
        Shot();
        UpdateAmmotext();
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
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            nextShot = Time.time + DelayShot;

            float startAngle = -totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (pelletsPerShot - 1);

            for (int i = 0; i < pelletsPerShot; i++)
            {
                float angleOffset = startAngle + angleStep * i;
                Quaternion pelletRotation = firePos.rotation * Quaternion.Euler(0, 0, angleOffset);
                Instantiate(bulletPrefabs, firePos.position, pelletRotation);
            }

            currentAmmo--;
            UpdateAmmotext();

            audioManager.playShotSound();     // Bắn
            audioManager.playReloadSound();   // Nhồi đạn (từng viên)

            transform.localPosition -= transform.right * recoilDistance;

            if (currentAmmo <= 0)
            {
                StartCoroutine(FullReload());
            }
        }
    }

    private System.Collections.IEnumerator FullReload()
    {
        isReloading = true;

        int steps = 3; // số lần nạp đạn
        float delayPerStep = 0.6f; // thời gian giữa mỗi tiếng

        for (int i = 0; i < steps; i++)
        {
            audioManager.playReloadSound();
            yield return new WaitForSeconds(delayPerStep);
        }

        currentAmmo = maxAmmo;
        UpdateAmmotext();
        isReloading = false;
    }
    public void SetOriginalLocalPos()
    {
        originalLocalPos = transform.localPosition;
    }



    void HandleRecoil()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
    }


    void UpdateAmmotext()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo > 0 ? currentAmmo.ToString() : "EMPTY";
    }

    public void AddAmmo(float amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        UpdateAmmotext();
    }

    public void SetEquipped(bool equipped) => isEquipped = equipped;
    public void SetAmmoText(TextMeshProUGUI text) => ammoText = text;
    public void SetAudioManager(AudioManager audio) => audioManager = audio;
}
