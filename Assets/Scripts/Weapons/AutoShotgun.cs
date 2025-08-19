﻿using TMPro;
using UnityEngine;

public class AutoShotgun : MonoBehaviour, IGun
{
    private float rotationOffSet = 180f;

    [Header("Thiết lập súng")]
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float delayShot = 0.25f; // tốc độ bắn nhanh hơn shotgun thường
    [SerializeField] private float maxAmmo = 10f;     // băng đạn 10 viên
    public float currentAmmo;
    private float nextShot;

    [SerializeField] public TextMeshProUGUI ammoText;
    public bool isEquipped = false;

    [Header("Spread")]
    [SerializeField] private int pelletsPerShot = 6; // bắn 6 viên cùng lúc
    [SerializeField] private float totalSpreadAngle = 15f; // độ lệch nhỏ hơn shotgun thường

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.1f; // nhẹ hơn shotgun
    [SerializeField] private float recoilReturnSpeed = 6f;

    private Vector3 originalLocalPos;

    [Header("Âm thanh riêng")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

    [Header("Reload Settings")]
    [SerializeField] private float reloadTime = 2.5f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isEquipped) return;

        HandleRecoil();

        if (isReloading) return;

        if (Time.time >= nextShot)
        {
            RotationGun();
            Shot();
        }

        // Reload khi nhấn R
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(ReloadCoroutine());
        }

        UpdateAmmoText();
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
        // Auto fire -> giữ chuột trái
        if (Input.GetMouseButton(0) && currentAmmo > 0)
        {
            nextShot = Time.time + delayShot;

            float startAngle = -totalSpreadAngle / 2f;
            float angleStep = totalSpreadAngle / (pelletsPerShot - 1);

            for (int i = 0; i < pelletsPerShot; i++)
            {
                float angleOffset = startAngle + angleStep * i;
                Quaternion pelletRotation = firePos.rotation * Quaternion.Euler(0, 0, angleOffset);
                Instantiate(bulletPrefabs, firePos.position, pelletRotation);
            }

            currentAmmo--;
            UpdateAmmoText();

            if (audioSource && shootClip)
                audioSource.PlayOneShot(shootClip);

            // recoil nhẹ hơn shotgun
            transform.localPosition -= transform.right * recoilDistance;
        }
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        if (audioSource && reloadClip)
            audioSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        UpdateAmmoText();

        isReloading = false;
    }

    void HandleRecoil()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
            ammoText.text = isReloading
                ? "RELOADING..."
                : (currentAmmo > 0 ? currentAmmo.ToString() : "EMPTY");
    }

    public void AddAmmo(float amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        UpdateAmmoText();
    }

    public void SetEquipped(bool equipped) => isEquipped = equipped;
    public void SetAmmoText(TextMeshProUGUI text) => ammoText = text;
    public void SetAudioManager(AudioManager audio) { }
}