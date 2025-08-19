using TMPro;
using UnityEngine;

public class Rifle : MonoBehaviour, IGun
{
    private float rotationOffSet = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float DelayShot = 0.1f;
    private float nextShot;

    [Header("Mana")]
    [SerializeField] private float manaCostPerShot = 5f; // mỗi phát bắn tốn mana
    private ManaSystem manaSystem; // gắn từ ngoài vào

    [Header("Bắn lệch")]
    [SerializeField] private float baseSpread = 1f;
    [SerializeField] private float distanceSpreadMultiplier = 0.1f;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.05f;
    [SerializeField] private float recoilReturnSpeed = 8f;

    [Header("Âm thanh riêng cho súng")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip emptyManaClip; // âm thanh khi hết mana

    public bool isEquipped = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isEquipped) return;

        HandleRecoil();
        RotationGun();
        AutoShot();
    }

    void RotationGun()
    {
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffSet);
        transform.localScale = (angle < -90 || angle > 90) ? Vector3.one : new Vector3(1, -1, 1);
    }

    void AutoShot()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShot)
        {
            if (CanShoot()) // dùng hàm interface
            {
                nextShot = Time.time + DelayShot;

                float distance = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                float totalSpread = baseSpread + distance * distanceSpreadMultiplier;
                float randomAngle = Random.Range(-totalSpread, totalSpread);

                Quaternion spreadRotation = firePos.rotation * Quaternion.Euler(0, 0, randomAngle);
                Instantiate(bulletPrefabs, firePos.position, spreadRotation);

                manaSystem.UseMana(manaCostPerShot);

                // Âm thanh bắn
                if (audioSource && shootClip)
                    audioSource.PlayOneShot(shootClip);

                // Giật súng (theo local X)
                transform.localPosition -= transform.right * recoilDistance;
            }
            else
            {
                // Hết mana thì play âm thanh báo
                if (audioSource && emptyManaClip)
                    audioSource.PlayOneShot(emptyManaClip);
            }
        }
    }

    void HandleRecoil()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
    }

    // ===============================
    // IGun interface implementation
    // ===============================
    public void SetEquipped(bool equipped) => isEquipped = equipped;
    public void SetAmmoText(TextMeshProUGUI text) { } // Rifle không dùng ammo
    public void SetAudioManager(AudioManager audio) { }
    public void AddAmmo(float amount) { } // Rifle không dùng ammo
    public bool CanShoot() => manaSystem != null && manaSystem.currentMana >= manaCostPerShot;
    public void SetManaSystem(ManaSystem mana) => manaSystem = mana;
}
