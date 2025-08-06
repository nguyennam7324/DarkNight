using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nameInput;

    public void GoToLogin()
    {
        //Chuyển scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }

    public void OnRegisterClick()
    {
        var email = emailInput.text;
        var password = passwordInput.text;
        var name = nameInput.text;

        var account = new Account
        {
            Email = email,
            Password = password,
            Name = name,
        };
        var json = JsonUtility.ToJson(account);
        Debug.Log(json);
        StartCoroutine(Post(json));
    }

    IEnumerator Post(string json)
    {

        var url = "http://localhost:5777/api/register";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var response = JsonUtility.FromJson<RegisterModeResponse>
            (request.downloadHandler.text);
            Debug.Log(response.status);
            if (response.status)
            {
                SceneManager.LoadScene("Login");
            }

        }


    }
}

