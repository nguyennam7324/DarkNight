using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene Instance;
    public GameObject m_LoadingScreenObject;
    public Slider ProgressBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            m_LoadingScreenObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StartGameWithLoading(GameManager gameManager)
    {
        // ẨN MAIN MENU TRƯỚC KHI HIỆN LOADING
        if (gameManager != null)
        {
            gameManager.HideMainMenu(); // Cần thêm method này
        }

        m_LoadingScreenObject.SetActive(true);
        ProgressBar.value = 0;
        StartCoroutine(LoadingCoroutine(gameManager));
    }

    IEnumerator LoadingCoroutine(GameManager gameManager)
    {
        Debug.Log("Loading started!");

        float loadTime = 2f;
        float currentTime = 0f;

        while (currentTime < loadTime)
        {
            // Sử dụng unscaledDeltaTime để không bị ảnh hưởng bởi timeScale
            currentTime += Time.unscaledDeltaTime;
            ProgressBar.value = currentTime / loadTime;
            yield return null;
        }

        ProgressBar.value = 1f;
        Debug.Log("Loading completed!");

        // SỬ DỤNG WaitForSecondsRealtime thay vì WaitForSeconds
        yield return new WaitForSecondsRealtime(0.3f);

        m_LoadingScreenObject.SetActive(false);
        Debug.Log("Loading screen hidden!");

        if (gameManager != null)
        {
            Debug.Log("Starting game...");
            gameManager.StartGame();
        }
        else
        {
            Debug.LogError("GameManager is null!");
        }
    }
}