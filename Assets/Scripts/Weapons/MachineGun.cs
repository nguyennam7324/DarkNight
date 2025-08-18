using UnityEngine;
using TMPro;

public class MachineGun : MonoBehaviour, IGun
{
    [Header("Thiết lập súng")]
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float delayShot = 0.07f;
    [SerializeField] private float maxAmmo = 128f;
    [SerializeField] private float reloadTime = 3.5f;

    [Header("Độ giật & Spread")]
    [SerializeField] private float recoilAmount = 0.15f;
    [SerializeField] private float spreadAngle = 6f;

    [Header("Âm thanh riêng cho súng")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    private float currentAmmo;
    private float nextShot;
    private bool isReloading = false;
    private bool isEquipped = false;

    private Vector3 originalLocalPos;

    private TextMeshProUGUI ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        originalLocalPos = transform.localPosition;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isEquipped || isReloading) return;

        RotationGun();
        Shot();
        Reload();
        UpdateAmmoText();
    }

    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        if (isEquipped)
            originalLocalPos = transform.localPosition;
    }

    public void SetAmmoText(TextMeshProUGUI text) => ammoText = text;

    // IGun bắt buộc phải có → nhưng MachineGun dùng audio riêng
    public void SetAudioManager(AudioManager manager)
    {
        // Không cần dùng
    }

    void RotationGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
            Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return;

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);

        if (angle < -90 || angle > 90)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(1, -1, 1);
    }

    void Shot()
    {
        if (Input.GetMouseButton(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + delayShot;
            Vector3 spread = firePos.eulerAngles;
            spread.z += Random.Range(-spreadAngle, spreadAngle);
            Instantiate(bulletPrefab, firePos.position, Quaternion.Euler(spread));

            currentAmmo--;
            ApplyRecoil();

            if (audioSource && shootClip)
                audioSource.PlayOneShot(shootClip);
        }
    }

    public void SetOriginalLocalPos()
    {
        originalLocalPos = transform.localPosition;
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private System.Collections.IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (audioSource && reloadClip)
            audioSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    private void ApplyRecoil()
    {
        Vector3 recoilDir = -firePos.right * recoilAmount;
        transform.localPosition = originalLocalPos + recoilDir;
        CancelInvoke(nameof(ResetRecoilPosition));
        Invoke(nameof(ResetRecoilPosition), 0.05f);
    }

    private void ResetRecoilPosition()
    {
        transform.localPosition = originalLocalPos;
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo > 0 ? currentAmmo.ToString() : "EMPTY";
        }
    }

    public void AddAmmo(float amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        UpdateAmmoText();
    }
}
