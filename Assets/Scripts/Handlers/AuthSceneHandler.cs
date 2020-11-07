using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Auth;

using Managers;

namespace Handlers
{
	public class AuthSceneHandler : MonoBehaviour
	{
	    private string gameSceneName = "Menu";

	    //Firebase variables
	    [Header("Firebase")]
	    public FirebaseUser User;

	    // Login variables
	    [Header("Login")]
	    public TMP_InputField emailLoginField;
	    public TMP_InputField passwordLoginField;
	    public TMP_Text warningLoginText;
	    public TMP_Text confirmLoginText;

	    // Register variables
	    [Header("Register")]
	    public TMP_InputField usernameRegisterField;
	    public TMP_InputField emailRegisterField;
	    public TMP_InputField passwordRegisterField;
	    public TMP_InputField passwordRegisterVerifyField;
	    public TMP_Text warningRegisterText;

	    //Function for the login button
	    public void LoginButton()
	    {
	        //Call the login coroutine passing the email and password
	        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
	    }
	    //Function for the register button
	    public void RegisterButton()
	    {
	        //Call the register coroutine passing the email, password, and username
	        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
	    }

	    private IEnumerator Login(string _email, string _password)
	    {
	        //Call the Firebase auth signin function passing the email and password
	        var LoginTask = MainManager.Instance.firebaseManager.auth.SignInWithEmailAndPasswordAsync(_email, _password);
	        //Wait until the task completes
	        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

	        if (LoginTask.Exception != null)
	        {
	            //If there are errors handle them
	            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
	            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
	            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

	            string message = "Login Failed!";
	            switch (errorCode)
	            {
	                case AuthError.MissingEmail:
	                    message = "Missing Email";
	                    break;
	                case AuthError.MissingPassword:
	                    message = "Missing Password";
	                    break;
	                case AuthError.WrongPassword:
	                    message = "Wrong Password";
	                    break;
	                case AuthError.InvalidEmail:
	                    message = "Invalid Email";
	                    break;
	                case AuthError.UserNotFound:
	                    message = "Account does not exist";
	                    break;
	            }
	            warningLoginText.text = message;
	        }
	        else
	        {
	            //User is now logged in
	            //Now get the result
	            User = LoginTask.Result;
	            Debug.LogFormat("User signed in successfully: {0} ({1}) having uid ({2})", User.DisplayName, User.Email, User.UserId);
	            warningLoginText.text = "";
	            confirmLoginText.text = "Logged In";

	            MainManager.Instance.currentUserName = User.DisplayName;
	            MainManager.Instance.currentUserId = User.UserId;

	            SceneManager.LoadScene(gameSceneName);
	        }
	    }

	    private IEnumerator Register(string _email, string _password, string _username)
	    {
	        if (_username == "")
	        {
	            //If the username field is blank show a warning
	            warningRegisterText.text = "Missing Username";
	        }
	        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
	        {
	            //If the password does not match show a warning
	            warningRegisterText.text = "Password Does Not Match!";
	        }
	        else
	        {
	            //Call the Firebase auth signin function passing the email and password
	            var RegisterTask = MainManager.Instance.firebaseManager.auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
	            //Wait until the task completes
	            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

	            if (RegisterTask.Exception != null)
	            {
	                //If there are errors handle them
	                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
	                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
	                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

	                string message = "Register Failed!";
	                switch (errorCode)
	                {
	                    case AuthError.MissingEmail:
	                        message = "Missing Email";
	                        break;
	                    case AuthError.MissingPassword:
	                        message = "Missing Password";
	                        break;
	                    case AuthError.WeakPassword:
	                        message = "Weak Password";
	                        break;
	                    case AuthError.EmailAlreadyInUse:
	                        message = "Email Already In Use";
	                        break;
	                }
	                warningRegisterText.text = message;
	            }
	            else
	            {
	                //User has now been created
	                //Now get the result
	                User = RegisterTask.Result;

	                if (User != null)
	                {
	                    //Create a user profile and set the username
	                    UserProfile profile = new UserProfile { DisplayName = _username };

	                    //Call the Firebase auth update user profile function passing the profile with the username
	                    var ProfileTask = User.UpdateUserProfileAsync(profile);
	                    //Wait until the task completes
	                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

	                    if (ProfileTask.Exception != null)
	                    {
	                        //If there are errors handle them
	                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
	                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
	                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
	                        warningRegisterText.text = "Username Set Failed!";
	                    }
	                    else
	                    {
	                        //Add the user to the firestore database.
	                        Dictionary<string, object> user = new Dictionary<string, object>
	                        {
	                            {"Username", User.DisplayName}
	                        };
	                        MainManager.Instance.firebaseManager.firestore.Collection("users").Document(User.UserId).SetAsync(user);

	                        //Username is now set
	                        //Now return to login screen
	                        UIManager.instance.LoginScreen();
	                        warningRegisterText.text = "";
	                    }
	                }
	            }
	        }
	    }
	}
}