using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button restartOnWinButton;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenuCanvas;
    GameManager _gameManager;
    AudioManager _audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
        if (_gameManager == null)
            Debug.LogError("Cannot find GameManager instance");
        if (_audioManager == null)
            Debug.LogError("Cannot find AudioManager instance");
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        restartButton.gameObject.SetActive(false);
        restartOnWinButton.gameObject.SetActive(false);
        LockScreenToLandscape();
    }

    void LockScreenToLandscape()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }

    void LoadMainMenuScene()
    {
        _gameManager.GetCurrency();
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        _gameManager.AddData();
        ReloadScene();
    }

    public void QuitGame()
    {
        _gameManager.GetCurrency();
        _gameManager.AddData();
        LoadMainMenuScene();
    }

    public void GoToSettingsMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenuCanvas.SetActive(true);
    }

    public void DisplayWinButton()
    {
        restartOnWinButton.gameObject.SetActive(true);
    }

    public void DisplayRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }
}
