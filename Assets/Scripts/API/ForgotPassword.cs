using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class ForgotPassword : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField emailInput;
    public TMP_InputField newPasswordInput;
    public TMP_InputField oldPasswordInput;

    void Start()
    {
        // Khởi tạo
    }

    public void OnSubmitClick()
    {
        var email = emailInput.text.Trim();
        var oldPassword = oldPasswordInput.text.Trim();
        var newPassword = newPasswordInput.text.Trim();

        // Kiểm tra input
        if (string.IsNullOrEmpty(email))
        {
            Debug.Log("Vui lòng nhập email");
            return;
        }

        if (string.IsNullOrEmpty(oldPassword))
        {
            Debug.Log("Vui lòng nhập mật khẩu cũ");
            return;
        }

        if (string.IsNullOrEmpty(newPassword))
        {
            Debug.Log("Vui lòng nhập mật khẩu mới");
            return;
        }

        if (oldPassword == newPassword)
        {
            Debug.Log("Mật khẩu mới phải khác mật khẩu cũ");
            return;
        }

        if (newPassword.Length < 6)
        {
            Debug.Log("Mật khẩu mới phải có ít nhất 6 ký tự");
            return;
        }

        // Tạo request object
        var request = new ChangePasswordRequest
        {
            Email = email,
            OldPassword = oldPassword,
            NewPassword = newPassword
        };

        var json = JsonUtility.ToJson(request);
        StartCoroutine(ChangePassword(json));
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    private void ShowMessage(string message, Color color)
    {
        Debug.Log($"Message: {message}");
    }

    IEnumerator ChangePassword(string json)
    {
        ShowMessage("Đang xử lý...", Color.yellow);

        var url = "http://localhost:5777/api/change-password";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Network Error: " + request.error);
            ShowMessage("Lỗi kết nối: " + request.error, Color.red);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            try
            {
                var response = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);

                if (response.status)
                {
                    ShowMessage("Đổi mật khẩu thành công! Đang chuyển về trang đăng nhập...", Color.green);
                    // Chờ 2 giây rồi chuyển về trang login
                    Invoke("GoToLogin", 2.0f);
                }
                else
                {
                    ShowMessage(response.message, Color.red);
                }
            }
            catch (Exception e)
            {
                Debug.Log("JSON Parse Error: " + e.Message);
                ShowMessage("Lỗi xử lý dữ liệu", Color.red);
            }
        }
    }

    // Thêm method để clear tất cả input fields
    public void ClearAllFields()
    {
        emailInput.text = "";
        oldPasswordInput.text = "";
        newPasswordInput.text = "";
    }
}

// Model cho API request
[Serializable]
public class ChangePasswordRequest
{
    public string Email;
    public string OldPassword;
    public string NewPassword;
}

// Model cho API response
[Serializable]
public class ApiResponse
{
    public bool status;
    public string message;
}