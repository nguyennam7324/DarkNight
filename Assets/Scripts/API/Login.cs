using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Login : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;


    public void GoToGame()
    {
        //Chuyển scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene1");
    }
    public void GoToForgotPassword()
    {
        SceneManager.LoadScene("ForgotPassword");
    }
    public void GotoResgister()
    {
        SceneManager.LoadScene("Register");
    }
    public void GotoLoginn()
    {
        SceneManager.LoadScene("Login");
    }

    public void OnLoginClick()
    {
        var email = emailInput.text;
        var password = passwordInput.text;


        var account = new Account
        {
            Email = email,
            Password = password,

        };
        var json = JsonUtility.ToJson(account);
        Debug.Log(json);
        StartCoroutine(Post(json));
    }

    IEnumerator Post(string json)
    {

        var url = "http://localhost:5777/api/login";
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
                SceneManager.LoadScene("Scene1");
            }

        }
        

    }
}
