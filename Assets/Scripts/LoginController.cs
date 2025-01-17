﻿using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }

    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    public static FirebaseUser User;

    private void Start()
    {
        signInButton.interactable = false;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                {
                    var result = task.Result;

                    if (result != DependencyStatus.Available)
                    {
                        Debug.LogError(result.ToString());
                        IsFirebaseReady = false;
                    }
                    else
                    {
                        IsFirebaseReady = true;

                        firebaseApp = FirebaseApp.DefaultInstance;
                        firebaseAuth = FirebaseAuth.DefaultInstance;
                    }

                    signInButton.interactable = IsFirebaseReady;
                }
            }
        );
    }

    public void LogInButtonClicked()
    {
        if(IsFirebaseReady && !IsSignInOnProgress && User == null)
        {
            IsSignInOnProgress = true;
            signInButton.interactable = false;

            firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
                {
                    //Debug.Log($"Sign in status : {task.Status}");

                    IsSignInOnProgress = false;
                    signInButton.interactable = true;

                    if (task.IsFaulted)
                    {
                        Debug.LogError(task.Exception);
                    }
                    else if (task.IsCanceled)
                    {
                        Debug.LogError("Sign-in canceled");
                    }
                    else
                    {
                        User = task.Result;
                        Debug.Log(User.Email);
                        SceneManager.LoadScene("Ready");
                    }
                }
            );
        }
    }

    public void SignUpButtonClicked()
    {
        SceneManager.LoadScene("SignUp");
    }
}
