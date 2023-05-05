using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button restartOnWinButton;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenuCanvas;
    [SerializeField] GameObject upgradeMenu;
    [SerializeField] GameObject controlMenuUI;
    [SerializeField] GameObject infoDisplayUI;
    [SerializeField] GameObject controlUI;
    GameManager _gameManager;
    AudioManager _audioManager;

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
        upgradeMenu.SetActive(false);
        controlUI.SetActive(true);
        controlMenuUI.SetActive(true);
        infoDisplayUI.SetActive(true);
        LockScreenToLandscape();
    }

    public void OpenUpgradeMenu()
    {
        _gameManager.GetCurrency();
        _gameManager.AddData();
        PauseGame();
        controlUI.SetActive(false);
        infoDisplayUI.SetActive(false);
        upgradeMenu.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        ResumeGame();
        upgradeMenu.SetActive(false);
        controlUI.SetActive(true);
        infoDisplayUI.SetActive(true);
        controlMenuUI.SetActive(true);
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
        if (restartOnWinButton != null)
            restartOnWinButton.gameObject.SetActive(true);
        else
            Debug.LogWarning("Reset On Win Button not found");
    }

    public void DisplayRestartButton()
    {
        if (restartOnWinButton != null)
            restartButton.gameObject.SetActive(true);
        else
            Debug.LogWarning("Reset Button not found");
    }
}
