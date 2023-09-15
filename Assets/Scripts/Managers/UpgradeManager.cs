using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] UpgradeStoreDisplay upgradeStoreDisplay;

    [SerializeField] List<UpgradeSO> upgradePool;
    List<UpgradeSO> availableUpgrades = new();
    Controller[] slots;

    Faction faction;
    Player player;
    int playerCurrency = 0;

    public int PlayerCurrency => playerCurrency;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        if (player == null)
            Debug.LogWarning("CouldNotFindPlayer");
        slots = player.GetComponentsInChildren<Controller>();
        foreach (Controller controller in slots)
        {
            Debug.Log(controller.name.ToString());
        }
        RefreshCurrency();
        StartCoroutine(ArenaSearch());
    }

    public void PreDock()
    {
        upgradeStoreDisplay.UpdateCurrency(playerCurrency);
        RefreshCurrency();
        RefreshUI();
    }

    void InitializeUI()
    {
        RefreshCurrency();
        upgradeStoreDisplay.InitializeStoreDisplay(availableUpgrades, playerCurrency);
        upgradeStoreDisplay.InitializeInstallsDisplay(slots);
    }

    void RefreshUI()
    {
        upgradeStoreDisplay.UpdateStoreDisplay(playerCurrency);
        upgradeStoreDisplay.UpdateCurrency(playerCurrency);
    }

    public void RefreshCurrency()
    {
        foreach (Controller controller in slots)
        {
            if(controller is CargoController cargoBay)
            {
                playerCurrency += cargoBay.CurrentCurrency;
                Debug.Log("Player deposited " + playerCurrency + " to station.");
                cargoBay.DepositCurrency();
            }

        }
    }

    public void TryBuyItem(UpgradeItemDisplay item)
    {
        UpgradeSO upgrade = item.GetUpgradeSO();
        if (CheckForAvailableSlot(upgrade))
        {
            if (playerCurrency >= (upgrade.BuyCost))
            {
                playerCurrency -= (upgrade.BuyCost);
                InstallItem(upgrade);
                upgradeStoreDisplay.UpdateInstallsDisplay(upgrade, false);
                upgradeStoreDisplay.UpdateStoreDisplay(playerCurrency);
                upgradeStoreDisplay.UpdateCurrency(playerCurrency);
            }
            else
                Debug.Log("Not enough scrap");
        }
        else
        {
            Debug.Log("No available slot for this item");
            DisplayError(item);
        }
    }

    public void SellItem(UpgradeItemDisplay item)
    {
        UpgradeSO upgrade = item.GetUpgradeSO();
        RemoveItem(upgrade);
        playerCurrency += upgrade.SellCost;
        upgradeStoreDisplay.UpdateInstallsDisplay(upgrade, true);
        upgradeStoreDisplay.UpdateStoreDisplay(playerCurrency);
        upgradeStoreDisplay.UpdateCurrency(playerCurrency);

    }

    Controller CheckForAvailableSlot(UpgradeSO upgrade)
    {
        foreach (Controller controller in slots)
        {
            if (upgrade.GetUpgradeType() == controller.GetSlotType() && controller.IsAvailable())
            {
                return controller;
            }
        }
        return null;
    }

    void GetRegionalAvailability()
    {
        foreach (UpgradeSO upgrade in upgradePool)
        {
            if (upgrade.UpgradeFaction == faction || upgrade.UpgradeFaction == Faction.Default)
            {
                availableUpgrades.Add(upgrade);
            }

        }
    }

    void RemoveItem(UpgradeSO upgrade)
    {
        foreach(Controller controller in slots)
        {
            if(controller.GetUpgrade() == upgrade)
            {
                controller.RemoveComponent();
            }
        }
    }

    void InstallItem(UpgradeSO upgradeItem)
    {
        Controller availableController = CheckForAvailableSlot(upgradeItem);    
        if (availableController != null)
            availableController.InstallComponent(upgradeItem);
    }

    public void BuyShipRepairs()
    {

    }

    public void DisplayInfo(UpgradeItemDisplay upgradeDis)
    {
        upgradeStoreDisplay.OpenInfoDisplay(upgradeDis.GetUpgradeSO());
    }

    public void CloseDisplayInfo()
    {
        upgradeStoreDisplay.CloseInfoDisplay();
    }

    public void DisplayError(UpgradeItemDisplay upgradeDis)
    {
        upgradeStoreDisplay.OpenErrorDisplay(upgradeDis.GetUpgradeSO());
    }

    public void CloseDisplayError()
    {
        upgradeStoreDisplay.CloseErrorDisplay();
    }

    IEnumerator ArenaSearch()
    {
        yield return new WaitForSeconds(3);
        faction = GameObject.Find("Arena").GetComponentInChildren<ArenaManager>().ArenaFaction;
        GetRegionalAvailability();
        InitializeUI();
    }
}
