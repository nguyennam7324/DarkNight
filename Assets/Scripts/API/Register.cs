using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    [Header("Register UI")]
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField confirmPasswordInputField;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Text statusText;

    private void Start()
    {
        registerButton.onClick.AddListener(HandleRegisterButtonClicked);
        backButton.onClick.AddListener(HandleBackButtonClicked);
    }

    private void HandleRegisterButtonClicked()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Please enter email and password";
            return;
        }

        if (password != confirmPassword)
        {
            statusText.text = "Passwords do not match";
            return;
        }

        if (password.Length < 6)
        {
            statusText.text = "Password must be at least 6 characters";
            return;
        }

        RegisterUser(email, password);
    }

    private async void RegisterUser(string email, string password)
    {
        try
        {
            statusText.text = "Registering...";

            // Đợi cho đến khi Firebase sẵn sàng
            await FirebaseAuthManager.Instance.Auth.CreateUserWithEmailAndPasswordAsync(email, password);

            statusText.text = "Registration successful!";

            // Gửi email xác thực
            if (FirebaseAuthManager.Instance.User != null)
            {
                await FirebaseAuthManager.Instance.User.SendEmailVerificationAsync();
                statusText.text = "Registration successful! Verification email sent.";
            }

            // Chuyển về màn hình login sau 2 giây
            Invoke(nameof(GoToLogin), 2f);
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
                case AuthError.EmailAlreadyInUse:
                    return "Email is already in use";
                case AuthError.WeakPassword:
                    return "Password is too weak";
                case AuthError.InvalidEmail:
                    return "Invalid email address";
                case AuthError.MissingEmail:
                    return "Please enter an email address";
                case AuthError.MissingPassword:
                    return "Please enter a password";
                default:
                    return "Registration failed. Please try again";
            }
        }

        return "Registration failed. Please try again";
    }

    private void HandleBackButtonClicked()
    {
        SceneManager.LoadScene("Login");
    }

    private void GoToLogin()
    {
        
    }

    public void ShowRegisterPanel()
    {
        registerPanel.SetActive(true);
        emailInputField.text = "";
        passwordInputField.text = "";
        confirmPasswordInputField.text = "";
        statusText.text = "";
    }
}