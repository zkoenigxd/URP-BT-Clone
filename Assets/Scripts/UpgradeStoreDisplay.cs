using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStoreDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text currencyDisplay;
    [SerializeField] Transform storeDisplayWindow;
    [SerializeField] Transform playerDisplayWindow;
    [SerializeField] UpgradeItemDisplay upgradeDisplayPrefab;
    [SerializeField] UpgradeItemDisplay installedUpgradePrefab;
    [SerializeField] Text currencyDisplayText;
    [SerializeField] TMP_Text errorDisplayText;
    [SerializeField] TMP_Text upgradeDisplayText;
    [SerializeField] GameObject infoDisplay;
    [SerializeField] GameObject errorDisplay;

    List<UpgradeItemDisplay> upgradeItemsInStore = new();
    List<UpgradeItemDisplay> upgradeItemsInstalled = new();

    private void Awake()
    {
        infoDisplay.SetActive(false);
        errorDisplay.SetActive(false);
    }

    public void UpdateCurrency(int scrap)
    {
        currencyDisplay.text = "Scrap: " + scrap;
    }

    public void InitializeStoreDisplay(List<UpgradeSO> upgrades, int playerCurency)
    {
        foreach (UpgradeSO upgrade in upgrades)
        {
            UpgradeItemDisplay displayItem = Instantiate(upgradeDisplayPrefab);
            upgradeItemsInStore.Add(displayItem);
            displayItem.transform.SetParent(storeDisplayWindow, false);
            displayItem.SetUpBuyDisplayTile(upgrade);
            if (upgrade.BuyCost <= playerCurency)
            {
                displayItem.SetBuyButtonActive();
            }
            else
            {
                displayItem.SetBuyButtonInactive();
                Debug.Log("Player currecy was " + playerCurency);
            }
        }
    }

    public void UpdateStoreDisplay(int playerCurrency)
    {
        foreach (UpgradeItemDisplay displayItem in upgradeItemsInStore)
        {
            if (displayItem.GetUpgradeSO().BuyCost <= playerCurrency)
            {
                displayItem.SetBuyButtonActive();
            }
            else
                displayItem.SetBuyButtonInactive();
        }
    }

    public void InitializeInstallsDisplay(Controller[] shipSlots)
    {
        foreach(Transform child in playerDisplayWindow.transform)
        {
            Destroy(child.gameObject);
        }
        upgradeItemsInstalled.Clear();
        foreach (Controller slot in shipSlots)
        {
            UpgradeItemDisplay displayItem = (UpgradeItemDisplay)Instantiate(installedUpgradePrefab);
            upgradeItemsInstalled.Add(displayItem);
            displayItem.transform.SetParent(playerDisplayWindow, false);
            if(slot.GetUpgrade() != null)
                displayItem.SetUpSellDisplayTile(slot.GetUpgrade());
            else
                displayItem.ClearSellDisplayTile();
        }
    }

    public void UpdateInstallsDisplay(UpgradeSO installedUpgrade, bool selling)
    {
        if (selling)
        {
            foreach (UpgradeItemDisplay displayItem in upgradeItemsInstalled)
            {
                if (displayItem.GetUpgradeSO() == installedUpgrade)
                {
                    displayItem.ClearSellDisplayTile();
                    break;
                }
            }
        }
        else
        {
            foreach (UpgradeItemDisplay nullItem in upgradeItemsInstalled)
            {
                if (nullItem.SlotType == installedUpgrade.GetUpgradeType() && nullItem.GetUpgradeSO() == null)
                {
                    nullItem.UpdateSellDisplayTile(installedUpgrade);
                    break;
                }
            }
        }
    }

    public void OpenInfoDisplay(UpgradeSO upgrade)
    {
        infoDisplay.SetActive(true);
        currencyDisplayText.text = upgrade.Name;
        upgradeDisplayText.text = upgrade.Description;
    }

    public void CloseInfoDisplay()
    {
        infoDisplay.SetActive(false);
    }

    public void OpenErrorDisplay(UpgradeSO upgrade)
    {
        errorDisplay.SetActive(true);
        errorDisplayText.text = "No available " + upgrade.GetUpgradeType() + " slot. To install a new module, you must first empty a " 
                              + upgrade.GetUpgradeType() + " slot by selling a " + upgrade.GetUpgradeType() + " currently installed on your ship.";
    }

    public void CloseErrorDisplay()
    {
        errorDisplay.SetActive(false);
    }
}
