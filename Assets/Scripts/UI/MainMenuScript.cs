using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    [SerializeField] Button startButton;
    [SerializeField] GameObject levelInfoText;


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
    string startingLocation;

    [System.Serializable]
    struct FactionInfo
    {
        public FactionSO factionInformation;
        public Button button;
    };

    [SerializeField] FactionInfo[] factions;

    GameManager _gameManager;
    AudioManager _audioManager;

    void OnEnable()
    {
        for (int i = 0; i < factions.Count(); ++i)
        {
            factions[i].button.gameObject.SetActive(false);
        }
        levelInfoText.SetActive(false);
    }

    void OnDisable()
    {
        //Un-Register Button Events
        for(int i = 0; i < factions.Count(); ++i)
        {
            factions[i].button.onClick.RemoveAllListeners();
        }
    }

    void Awake()
    {
        GoToLogin();
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

    public void SetStartSector(int ID)
    {
        Debug.Log(ID);
        levelInfoText.SetActive(true);
        if (_gameManager.IsSectorUnlocked(ID))
        {
            startingLocation = factions[ID].factionInformation.startLevelKey;
            startButton.interactable = true;
             levelInfoText.GetComponent<StartLevelInfoDisplay>().UpdateInfo(factions[ID].factionInformation, true);
        }
        else
        {
            startingLocation = null;
            startButton.interactable = false;
            if (_gameManager.IsSectorDiscovered(ID))
                levelInfoText.GetComponent<StartLevelInfoDisplay>().UpdateInfo(factions[ID].factionInformation, false);
            else
                levelInfoText.GetComponent<StartLevelInfoDisplay>().UpdateInfo();
        }

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
        _gameManager.LoadNewGame(startingLocation);
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

    public void LoadStartMenu()
    {
        registrationMenuCanvas.SetActive(false);
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
        StartCoroutine(DisplayButtonsAfterLoad());
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


    IEnumerator DisplayButtonsAfterLoad()
    {
        yield return new WaitUntil(() => _gameManager.LoadComplete);
        for (int i = 0; i < factions.Count(); ++i)
        {
            Debug.Log("Attempting loop " + i);
            factions[i].button.gameObject.SetActive(true);
            if (_gameManager.IsSectorDiscovered(i))
            {
                factions[i].button.GetComponentInChildren<TMP_Text>().text = factions[i].factionInformation.factionName;
                factions[i].button.GetComponent<Image>().color = factions[i].factionInformation.mainColor;
            }
            else
            {
                factions[i].button.GetComponentInChildren<TMP_Text>().text = "???";
                factions[i].button.GetComponent<Image>().color = Color.gray;
            }
            int x = i;
            factions[i].button.onClick.AddListener(delegate { SetStartSector(x); });
            Debug.Log("Loop" + i + "complete");
        }
    }
}
