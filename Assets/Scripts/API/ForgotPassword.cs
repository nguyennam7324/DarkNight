using Firebase;
using Firebase.Auth;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ForgotPassword : MonoBehaviour
{
    [Header("Forgot Password UI")]
    [SerializeField] private GameObject forgotPasswordPanel;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private Button resetPasswordButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Text statusText;
    private int countdown = 30;

    private void Start()
    {
        resetPasswordButton.onClick.AddListener(HandleResetPasswordButtonClicked);
        backButton.onClick.AddListener(HandleBackButtonClicked);
    }

    private void HandleResetPasswordButtonClicked()
    {
        string email = emailInputField.text;

        if (string.IsNullOrEmpty(email))
        {
            statusText.text = "Please enter your email address";
            return;
        }

        ResetPassword(email);
    }

    private async void ResetPassword(string email)
    {
        try
        {
            statusText.text = "Sending reset email...";

            await FirebaseAuth.DefaultInstance.SendPasswordResetEmailAsync(email);

            statusText.text = "Password reset email sent! Check your inbox.";

            DisableButtons();
        }
        catch (Exception e)
        {
            statusText.text = HandleFirebaseError(e);
            Debug.LogError(e);
        }
    }

    private void DisableButtons()
    {
        StartCoroutine(DisableButtonsCoroutine());
    }

    private IEnumerator DisableButtonsCoroutine()
    {
        resetPasswordButton.interactable = false;
        InvokeRepeating("CountDownReset", 0, 1f);
        yield return new WaitForSeconds(countdown);
        CancelInvoke("CountDownReset");
        resetPasswordButton.interactable = true;
    }

    private void CountDownReset()
    {
        countdown--;
        resetPasswordButton.interactable = true;
        statusText.text = "Password reset email sent! Check your inbox..." + countdown;
    }

    private string HandleFirebaseError(Exception exception)
    {
        FirebaseException firebaseEx = exception as FirebaseException;
        if (firebaseEx != null)
        {
            var errorCode = (AuthError)firebaseEx.ErrorCode;

            switch (errorCode)
            {
                case AuthError.InvalidEmail:
                    return "Invalid email address";
                case AuthError.UserNotFound:
                    return "No account found with this email";
                default:
                    return "Failed to send reset email. Please try again";
            }
        }

        return "Failed to send reset email. Please try again";
    }

    private void HandleBackButtonClicked()
    {
        SceneManager.LoadScene("Login");
    }

    private void GoToLogin()
    {
        
    }

    public void ShowForgotPasswordPanel()
    {
        forgotPasswordPanel.SetActive(true);
        emailInputField.text = "";
        statusText.text = "";
    }
}