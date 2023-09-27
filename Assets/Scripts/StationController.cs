using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{
    [SerializeField] GameObject redArrowAnim;
    [SerializeField] GameObject greenArrowAnim;
    UpgradeManager upgradeManager;
    UIManager UImanager;
    Button startUpgradeButton;
    TMP_Text messageBox;
    bool isSafe;
    bool hasEntered;

    private void Awake()
    {
        messageBox = GameObject.Find("NPC Diologe").GetComponent<TMP_Text>();
        startUpgradeButton = GameObject.Find("StartUpgrade").GetComponent<Button>();
        upgradeManager = FindAnyObjectByType<UpgradeManager>();
        startUpgradeButton.enabled = false;
        startUpgradeButton.transform.GetChild(0).gameObject.SetActive(false);
        startUpgradeButton.GetComponent<Image>().enabled = false;
        messageBox.enabled = false;
        UImanager = FindObjectOfType<UIManager>();
        hasEntered = false;
        isSafe = false;
        ArenaIsNotSafe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>() && !hasEntered)
        {
            hasEntered = true;
            if(isSafe)
            {
                startUpgradeButton.enabled = true;
                startUpgradeButton.GetComponent<Image>().enabled = true;
                startUpgradeButton.transform.GetChild(0).gameObject.SetActive(true);
                DisplayMissionCompleteMessage();
                upgradeManager.PreDock();
            }
            else
            {
                DisplayMissionIncompleteMessage();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>())
        {
            startUpgradeButton.enabled = false;
            startUpgradeButton.GetComponent<Image>().enabled = false;
            startUpgradeButton.transform.GetChild(0).gameObject.SetActive(false);
            hasEntered = false;
            CloseMessage();
        }
    }

    void DisplayMissionIncompleteMessage()
    {
        messageBox.enabled = true;
        messageBox.text = "Cannot Dock Until All Enemies Destroyed";
    }

    void DisplayMissionCompleteMessage()
    {
        messageBox.enabled = true;
        messageBox.text = "Scrap Deposited to Station.\nDock to Upgrade Ship";
    }

    void CloseMessage()
    {
        messageBox.text = " ";
        messageBox.enabled = false;
    }

    public void ArenaIsSafe()
    {
        Debug.Log("Arena Safe");
        isSafe = true;
        redArrowAnim.SetActive(false);
        greenArrowAnim.SetActive(true);
        messageBox.enabled = true;
        messageBox.text = "Local Space Station Dock Now Open";
    }

    public void ArenaIsNotSafe()
    {
        Debug.Log("Arena Not Safe");
        isSafe = false;
        redArrowAnim.SetActive(true);
        greenArrowAnim.SetActive(false);
    }
}
