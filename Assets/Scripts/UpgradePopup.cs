using UnityEngine;
using TMPro;
using System.Collections;

public class UpgradePopup : MonoBehaviour
{
    public static UpgradePopup Instance;

    public TextMeshProUGUI popupText;
    public CanvasGroup canvasGroup;
    public float displayTime = 1.5f;
    public float fadeTime = 0.5f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Đảm bảo luôn bắt đầu với alpha = 0
        canvasGroup.alpha = 0f;
    }

    public void Show(string message)
    {
        Debug.Log("Pup show" + message);
        popupText.text = message;

        // Reset trạng thái alpha và hiển thị
        canvasGroup.alpha = 1f;
        popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, 1f);

        // KHÔNG SetActive lại, tránh lỗi Coroutine
        StopAllCoroutines();
        StartCoroutine(ShowAndFade());


        
    }

    private IEnumerator ShowAndFade()
    {
        float timer = 0f;

        // Giữ nguyên trạng thái trong thời gian hiển thị
        while (timer < displayTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Mờ dần
        timer = 0f;
        while (timer < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}