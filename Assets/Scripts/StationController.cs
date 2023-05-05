using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{
    [SerializeField] TMP_Text messageBox;
    [SerializeField] GameObject redArrowAnim;
    [SerializeField] GameObject greenArrowAnim;
    [SerializeField] Button startUpgradeButton;
    UpgradeManager upgradeManager;
    UIManager UImanager;
    bool isSafe;
    bool hasEntered;

    private void Awake()
    {
        upgradeManager = FindAnyObjectByType<UpgradeManager>();
        startUpgradeButton.gameObject.SetActive(false);
        messageBox.enabled = false;
        UImanager = FindObjectOfType<UIManager>();
        hasEntered = false;
        isSafe = false;
        ArenaIsNotSafe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && !hasEntered)
        {
            hasEntered = true;
            if(isSafe)
            {
                startUpgradeButton.gameObject.SetActive(true);
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
        if (collision.gameObject.GetComponent<Player>())
        {
            startUpgradeButton.gameObject.SetActive(false);
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
