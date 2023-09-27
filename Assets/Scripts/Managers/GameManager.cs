using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager>
{

    MainMenuScript _mainMenuScript;
    FirebaseAuth auth;
    DatabaseReference dataRef;
    Vector2 entryVector = Vector2.right;
    UpgradeManager upgradeManager;
    UserSaveData saveData;

    string currentUserID = null;
    string currentUser = "Error, user not set";

    bool enteringNewArena = true;
    bool loadComplete;
    public bool loggedIn;

    int currencyCollected = 0;

    public Vector2 EntryVector => entryVector;
    public bool EnteringNewArena => enteringNewArena;
    public bool LoadComplete => loadComplete;

    protected override void OnAwake()
    {
        // Firebase Initializations
        auth = FirebaseAuth.DefaultInstance;
        Debug.Log(auth);
        dataRef = FirebaseDatabase.DefaultInstance.RootReference;
        _mainMenuScript = FindObjectOfType<MainMenuScript>();
        if (currentUserID == null) { loggedIn = false; }
        else { loggedIn = true; }
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

            string message = errorCode.ToString();


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
        saveData = new UserSaveData(currentUser);
        Debug.Log(currencyCollected);
        string json = JsonUtility.ToJson(saveData);
        await dataRef.Child("users").Child(currentUserID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            Debug.Log(currentUser + "'s data added successfully " + currencyCollected);
        });
    }

    public void GetUserData()
    {
        dataRef.Child("users").Child(currentUserID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            StartCoroutine(PlayLoadScreen());
            Debug.Log("entering snapshot");
            if (task.IsFaulted)
            {
                Debug.Log(currentUser + " data retrieval fail with Exception: " + task.Exception);
                // TODO: Handle exception on user side.
            }
            else if (task.IsCompleted)
            {
                DataSnapshot jsonReturn = task.Result;
                string json = jsonReturn.GetRawJsonValue();
                Debug.Log(json);
                saveData = JsonUtility.FromJson<UserSaveData>(json);
                Debug.Log(saveData.ToString());
            }
            currentUser = saveData.userName;
            loadComplete = true;
            Debug.Log("Load Complete");
        });
    }

    public void ClearUserData()
    {
        currentUser = " ";
        currentUserID = " ";
        currencyCollected = 0;
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

    public void LoadNewGame(string startinglevel)
    {
        SceneManager.LoadScene("LevelUtilities");
        SceneManager.LoadScene(startinglevel, LoadSceneMode.Additive);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }


    public bool IsSectorUnlocked(int ID)
    {
        return saveData.sectorUnlockedMatrix[ID];
    }

    public bool IsSectorDiscovered(int ID)
    {
        return saveData.sectorFoundMatrix[ID];
    }


    public class UserSaveData
    {
        public string userName;
        public string shipClass;
        public int shipLevel;
        public bool shipHasHullUpgrade;
        public int scrapInInventory;
        public bool[] sectorFoundMatrix;
        public bool[] sectorUnlockedMatrix;

        public UserSaveData(string currentUserName)
        {
            userName = currentUserName;
            shipClass = "Destroyer";
            sectorFoundMatrix = new bool[6];
            sectorUnlockedMatrix = new bool[6];
            sectorFoundMatrix[0] = true;
            sectorFoundMatrix[1] = true;
            sectorFoundMatrix[2] = true;
            sectorUnlockedMatrix[0] = true;
            sectorUnlockedMatrix[1] = true;
        }

        public UserSaveData(UserSaveData other)
        {
            if (other != null)
            {
                this.userName = other.userName;
                this.shipClass = other.shipClass;
                this.shipLevel = other.shipLevel;
                this.shipHasHullUpgrade = other.shipHasHullUpgrade;
                this.scrapInInventory = other.scrapInInventory;
                this.sectorFoundMatrix = other.sectorFoundMatrix;
                this.sectorUnlockedMatrix = other.sectorUnlockedMatrix;
            }
        }

    }

    IEnumerator PlayLoadScreen()
    {
        yield return null;
    }

}
