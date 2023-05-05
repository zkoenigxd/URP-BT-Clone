using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager>
{

    MainMenuScript _mainMenuScript;
    FirebaseAuth auth;
    DocumentReference docRef;
    FirebaseFirestore db;
    Vector2 entryVector = Vector2.up;
    Player player;
    UpgradeManager upgradeManager;

    int myLevel = 0;

    string currentUserID = null;
    string currentUser = "Error, user not set";

    bool enteringNewArena = true;
    bool loadComplete;
    public bool loggedIn;

    int currencyCollected = 0;

    public Vector2 EntryVector => entryVector;
    public bool EnteringNewArena => enteringNewArena;

    protected override void OnAwake()
    {
        // Firebase Initializations
        auth = FirebaseAuth.DefaultInstance;
        Debug.Log(auth);
        db = FirebaseFirestore.DefaultInstance;
        _mainMenuScript = FindObjectOfType<MainMenuScript>();
        if (currentUserID == null) { loggedIn = false; }
        else { loggedIn = true; }
        player = FindObjectOfType<Player>();
    }

    public void ResetCurrency()
    {

    }

    public void GetCurrency()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
        if (upgradeManager != null)
            currencyCollected += upgradeManager.PlayerCurrency;
    }

    private IEnumerator LoginAsync()
    {
        string email = _mainMenuScript.ifUserInput.text;
        string password = _mainMenuScript.ifPasswordInput.text;

        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        if (LoginTask.Exception != null)
        {
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
            _mainMenuScript.startMenuText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            FirebaseUser User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);

            currentUserID = User.UserId;

            _mainMenuScript.GoToStartMenu();
            GetUserData();
            yield return new WaitUntil(() => loadComplete);
            loadComplete = false;
            DisplayUserData();
            loggedIn = true;
        }
    }

    public void CreateNewUser()
    {
        string username = _mainMenuScript.ifRegisterUsername.text;
        string email = _mainMenuScript.ifRegisterUserInput.text;
        string password = _mainMenuScript.ifRegisterPasswordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                _mainMenuScript.textRegTop.text = "Cancelled";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                _mainMenuScript.textRegTop.text = "Unable to authenticate";
                return;
            }

            FirebaseUser newUser = task.Result;
            currentUserID = newUser.UserId;

            _mainMenuScript.textRegTop.text = "Registration successful";

            currentUser = username;
            AddData();
            _mainMenuScript.GoToLogin();
            _mainMenuScript.startMenuText.text = "Registration Successful";
        });
    }

    async public void AddData()
    {
        Debug.Log(currencyCollected);
        DocumentReference docRef = db.Collection("TestUnity").Document(currentUserID);
        Dictionary<string, object> stats = new ()
        {
            { "Name", currentUser },
            { "Scrap Collected", currencyCollected },
            { "Ship Class", myLevel }
        };
        await docRef.SetAsync(stats).ContinueWithOnMainThread(task => { Debug.Log(currentUser + "'s data added successfully " + currencyCollected); });
    }

    public void GetUserData()
    {
        docRef = db.Collection("TestUnity").Document(currentUserID);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            Debug.Log("entering snapshot");
            if (snapshot.Exists)
            {
                Dictionary<string, object> userInfo = snapshot.ToDictionary();

                foreach (KeyValuePair<string, object> pair in userInfo)
                {

                    Debug.Log(pair.Key);
                    if (pair.Key.ToString().Trim() == "Scrap Collected")
                        currencyCollected = Convert.ToInt32(pair.Value);
                    else if (pair.Key.ToString().Trim() == "Ship Class")
                        myLevel = Convert.ToInt32(pair.Value);
                    else if (pair.Key.ToString().Trim() == "Name")
                        currentUser = Convert.ToString(pair.Value);
                }
                Debug.Log(currentUser);
            }
            else
            {
                Debug.Log("Error Getting Firestore Value");
            }
            loadComplete = true;
        });
        return;
    }

    public void ClearUserData()
    {
        currentUser = " ";
        currentUserID = " ";
        currencyCollected = 0;
        myLevel = 0;
        loggedIn = false;
    }

    public void DisplayUserData()
    {
        _mainMenuScript = FindObjectOfType<MainMenuScript>();
        if (_mainMenuScript != null)
        {
            _mainMenuScript.textUser.text = currentUser;
            _mainMenuScript.textGamePoints.text = "Highscore: " + currencyCollected;
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync());
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
