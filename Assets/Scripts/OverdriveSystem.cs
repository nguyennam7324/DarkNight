using UnityEngine;
using System;

public class OverdriveSystem : MonoBehaviour
{
    public event Action OnOverdriveStart;
    public event Action OnOverdriveEnd;

    [Header("Overdrive Settings")]
    public float overdriveDuration = 5f;
    private bool isActive = false;
    private float timer;
    [SerializeField] private float overdrivePerKill = 10f;
    [SerializeField] private float maxOverdrive = 100f;
    [SerializeField] private float overdrivePerHit = 5f;
    private float currentOverdrive = 0f;
    [SerializeField] private GameObject overdriveEffectPrefab;
    [SerializeField] private Transform effectSpawnPoint; // chỗ spawn effect (thường là vị trí player)


    [Header("Optional VFX & SFX")]
    public OverdriveVFX overdriveVFX; // Gắn script OverdriveVFX vào đây

    /// <summary>
    /// Gọi hàm này để kích hoạt Overdrive
    /// </summary>
    private void TriggerOverdrive()
{
    Debug.Log("⚡ Overdrive Activated! ⚡");

    // Spawn effect nếu có prefab
    if (overdriveEffectPrefab != null && effectSpawnPoint != null)
    {
        GameObject fx = Instantiate(overdriveEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
        Destroy(fx, 2f); // Tự hủy effect sau 2 giây
    }

    // Reset Overdrive sau khi kích hoạt
    currentOverdrive = 0f;

    // Thêm buff tạm thời hoặc damage boost ở đây nếu cần
}
    private void Start()
    {

    }


    private void Update()
    {
        if (isActive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                EndOverdrive();
            }
        }
    }

    /// <summary>
    /// Kết thúc Overdrive
    /// </summary>
    private void EndOverdrive()
    {
        isActive = false;

        // Gọi sự kiện kết thúc
        OnOverdriveEnd?.Invoke();

        // Tắt hiệu ứng
        if (overdriveVFX != null)
            overdriveVFX.Stop();

        Debug.Log("❄ Overdrive Ended!");
    }   
    public void OnEnemyKilled()
{
    // Ví dụ: tăng điểm Overdrive khi giết enemy
    currentOverdrive += overdrivePerKill; // overdrivePerKill là số cộng mỗi kill
    currentOverdrive = Mathf.Clamp(currentOverdrive, 0, maxOverdrive);

    // Nếu đạt max thì kích hoạt Overdrive
    if (currentOverdrive >= maxOverdrive)
    {
        TriggerOverdrive();
    }
}
    public void OnEnemyHit()
{
    // Tăng Overdrive mỗi lần bắn trúng kẻ địch
    currentOverdrive += overdrivePerHit;
    currentOverdrive = Mathf.Clamp(currentOverdrive, 0, maxOverdrive);

    // Nếu đầy thì kích hoạt Overdrive
    if (currentOverdrive >= maxOverdrive)
    {
        TriggerOverdrive();
    }
}
}
