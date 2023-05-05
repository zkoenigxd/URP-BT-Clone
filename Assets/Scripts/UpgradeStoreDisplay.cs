using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeStoreDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text currencyDisplay;
    [SerializeField] Transform storeDisplayWindow;
    [SerializeField] Transform playerDisplayWindow;
    [SerializeField] UpgradeItemDisplay upgradeDisplayPrefab;
    [SerializeField] UpgradeItemDisplay installedUpgradePrefab;

    List<UpgradeItemDisplay> upgradeItemsInStore = new();
    List<UpgradeItemDisplay> upgradeItemsInstalled = new();

    public void UpdateCurrency(int scrap)
    {
        currencyDisplay.text = "Scrap: " + scrap;
    }

    public void InitializeStoreDisplay(List<UpgradeSO> upgrades, int playerCurency)
    {
        foreach (UpgradeSO upgrade in upgrades)
        {
            UpgradeItemDisplay displayItem = (UpgradeItemDisplay)Instantiate(upgradeDisplayPrefab);
            upgradeItemsInStore.Add(displayItem);
            displayItem.transform.SetParent(storeDisplayWindow, false);
            displayItem.SetUpBuyDisplayTile(upgrade);
            if(upgrade.BuyCost <= playerCurency )
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

    public void InitializeInstallsDisplay(List<UpgradeSO> upgrades)
    {
        foreach (UpgradeSO upgrade in upgrades)
        {
            UpgradeItemDisplay displayItem = (UpgradeItemDisplay)Instantiate(installedUpgradePrefab);
            upgradeItemsInstalled.Add(displayItem);
            displayItem.transform.SetParent(playerDisplayWindow, false);
            displayItem.SetUpSellDisplayTile(upgrade);
        }
    }

    public void UpdateInstallsDisplay(UpgradeSO installedUpgrade, bool selling)
    {
        if (selling)
        {
            foreach(UpgradeItemDisplay displayItem in upgradeItemsInstalled)
            {
                if(displayItem.GetUpgradeSO() ==  installedUpgrade)
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
                if (nullItem.SlotType == installedUpgrade.UpgradeType && nullItem.GetUpgradeSO() == null)
                {
                    nullItem.UpdateSellDisplayTile(installedUpgrade);
                    break;
                }
            }
        }
    }







    //public void UpdateStoreDisplay(UpgradeItemDisplay purchasedItem, int playerCurrency)
    //{
    //    if (purchasedItem != null)
    //    {
    //        upgradeItemsInStore.Remove(purchasedItem);
    //        Destroy(purchasedItem.gameObject);
    //    }
    //    foreach (UpgradeItemDisplay displayItem in upgradeItemsInStore)
    //    {
    //        if (displayItem.GetUpgradeSO().BuyCost <= playerCurrency)
    //        {
    //            displayItem.SetBuyButtonActive();
    //        }
    //        else
    //            displayItem.SetBuyButtonInactive();
    //    }
    //}
}
