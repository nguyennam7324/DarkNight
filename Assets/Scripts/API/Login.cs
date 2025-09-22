using Firebase;
using Firebase.Auth;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [Header("Login UI")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button forgotPasswordButton;
    [SerializeField] private TMP_Text statusText;

    private void Start()
    {
        loginButton.onClick.AddListener(HandleLoginButtonClicked);
        registerButton.onClick.AddListener(HandleRegisterButtonClicked);
        forgotPasswordButton.onClick.AddListener(HandleForgotPasswordButtonClicked);
    }

    private void HandleLoginButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Please enter email and password";
            return;
        }

        LoginUser(email, password);
    }

    private async void LoginUser(string email, string password)
    {
        try
        {
            statusText.text = "Logging in...";

            // Đợi cho đến khi Firebase sẵn sàng
            await FirebaseAuthManager.Instance.Auth.SignInWithEmailAndPasswordAsync(email, password);

            // Kiểm tra xem email đã được xác thực chưa
            if (FirebaseAuthManager.Instance.User != null)
            {
                statusText.text = "Login successful!";
                // Chuyển đến scene chính của game
                SceneManager.LoadScene("Scene1");
            }
            else 
            {
                FirebaseAuthManager.Instance.Auth.SignOut();
            }
        }
        catch (Exception e)
        {
            statusText.text = HandleFirebaseError(e);
            Debug.LogError(e);
        }
    }

    private string HandleFirebaseError(Exception exception)
    {
        FirebaseException firebaseEx = exception as FirebaseException;
        if (firebaseEx != null)
        {
            var errorCode = (AuthError)firebaseEx.ErrorCode;

            switch (errorCode)
            {
                case AuthError.WrongPassword:
                    return "Wrong password";
                case AuthError.InvalidEmail:
                    return "Invalid email address";
                case AuthError.UserNotFound:
                    return "Account not found";
                case AuthError.TooManyRequests:
                    return "Too many attempts. Try again later";
                default:
                    return "Login failed. Please try again";
            }
        }

        return "Login failed. Please try again";
    }

    private void HandleRegisterButtonClicked()
    {
        SceneManager.LoadScene("Register");
    }

    private void HandleForgotPasswordButtonClicked()
    {
        SceneManager.LoadScene("FogotAndChange");
    }

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        emailInputField.text = "";
        passwordInputField.text = "";
        statusText.text = "";
    }
}