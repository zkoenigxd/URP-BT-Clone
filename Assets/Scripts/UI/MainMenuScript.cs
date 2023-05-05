using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject startMenuCanvas;
    [SerializeField] GameObject registrationMenuCanvas;
    [SerializeField] GameObject gameMenuCanvas;
    [SerializeField] GameObject settingsMenuCanvas;

    [SerializeField] GameObject backgroundPanel;
    public TMP_Text startMenuText;
    public Toggle toggle;

    public TMP_Text textRegTop;
    public TMP_InputField ifRegisterUsername;
    public TMP_InputField ifUserInput;
    public TMP_InputField ifPasswordInput;

    public TMP_InputField ifRegisterUserInput;
    public TMP_InputField ifRegisterPasswordInput;

    public TMP_Text textUser;
    public TMP_Text textGameLevel;
    public TMP_Text textGamePoints;

    int toggleInt;

    GameManager _gameManager;
    AudioManager _audioManager;

    void Awake()
    {
        _audioManager = AudioManager.Instance;
        _gameManager = GameManager.Instance;
        Debug.Log(_gameManager.ToString());
        Debug.Log(_gameManager.loggedIn.ToString());

        if (_gameManager.loggedIn)
        {
            GoToStartMenu();
            _gameManager.DisplayUserData();
        }
        else
        {
            toggleInt = PlayerPrefs.GetInt("Remember Login", 0);
            if (toggleInt != 0)
            {
                ifUserInput.text = PlayerPrefs.GetString("Email", null);
                ifPasswordInput.text = PlayerPrefs.GetString("Password", null);
                Login();
            }
            else
            {
                _gameManager.ClearUserData();
                GoToLogin();
            }

        }
        AllowAutoRotation();
    }

    void AllowAutoRotation()
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    public void Login()
    {
        SetPrefs();
        _gameManager.Login();
    }

    public void Logout()
    {
        _gameManager.ClearUserData();
    }

    public void RegisterNewUser()
    {
        _gameManager.CreateNewUser();
    }

    public void StartGame()
    {
        _gameManager.LoadNewGame();
    }

    public void GoToLogin()
    {
        startMenuCanvas.SetActive(true);
        registrationMenuCanvas.SetActive(false);
        gameMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
    }

    public void GoToRegistration()
    {
        registrationMenuCanvas.SetActive(true);
        startMenuCanvas.SetActive(false);
        gameMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
    }

    public void GoToStartMenu()
    {
        startMenuCanvas.SetActive(false);
        registrationMenuCanvas.SetActive(false);
        gameMenuCanvas.SetActive(true);
        settingsMenuCanvas.SetActive(false);
    }

    public void GoToSettingsMenu()
    {
        settingsMenuCanvas.SetActive(true);
    }

    private void SetPrefs()
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt("Remember Login", 1);
            PlayerPrefs.SetString("Email", ifUserInput.text);
            PlayerPrefs.SetString("Password", ifPasswordInput.text);
        }
        else
        {
            PlayerPrefs.SetInt("Remember Login", 0);
            PlayerPrefs.SetString("Email", null);
            PlayerPrefs.SetString("Password", null);
        }
    }
}
