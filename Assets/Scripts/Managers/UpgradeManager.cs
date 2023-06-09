using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] UpgradeStoreDisplay upgradeStoreDisplay;

    [SerializeField] List<WeaponUSO> weaponUpgrades = new();
    [SerializeField] List<ShieldUSO> shieldUpgrades = new();
    [SerializeField] List<PowerUSO> powerUpgrades = new();
    [SerializeField] List<CargoUSO> cargoUpgrades = new();

    List<UpgradeSO> availableUpgrades = new();

    WeaponController[] weaponControllers;
    ShieldController shieldController;
    CurrecyController cargoController;
    PowerController powerController;

    List<UpgradeSO> installedUpgrades = new();

    Faction faction;
    Player player;
    int playerCurrency = 0;

    public int PlayerCurrency => playerCurrency;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        if (player == null)
            Debug.LogWarning("CouldNotFindPlayer");
        cargoController = player.GetComponentInChildren<CurrecyController>();
        powerController = player.GetComponentInChildren<PowerController>();
        shieldController = player.GetComponentInChildren<ShieldController>();
        weaponControllers = player.GetComponentsInChildren<WeaponController>();
        if (cargoController == null || powerController == null || shieldController == null || weaponControllers == null)
            Debug.LogWarning("Did not find controllers");
        faction = FindObjectOfType<ArenaManager>().ArenaFaction;
        RefreshCurrancy();
        upgradeStoreDisplay.UpdateCurrency(playerCurrency);
        GetRegionalAvailability();
        InitializePlayerInstalledUpgradesList();
        InitializeUI();
    }

    public void PreDock()
    {
        RefreshCurrancy();
        RefreshUI();
    }

    void InitializeUI()
    {
        RefreshCurrancy();
        upgradeStoreDisplay.InitializeStoreDisplay(availableUpgrades, playerCurrency);
        upgradeStoreDisplay.InitializeInstallsDisplay(installedUpgrades);
    }

    void RefreshUI()
    {
        upgradeStoreDisplay.UpdateStoreDisplay(playerCurrency);
        upgradeStoreDisplay.UpdateCurrency(playerCurrency);
    }

    public void RefreshCurrancy()
    {
        playerCurrency += cargoController.CurrentCurrency;
        Debug.Log("Player deposited " + playerCurrency + " to station.");
        cargoController.DepositCurrency();
    }

    public void TryBuyItem(UpgradeItemDisplay item)
    {
        UpgradeSO upgrade = item.GetUpgradeSO();
        if (CheckForAvailableSlot(upgrade.UpgradeType))
        {
            if (playerCurrency >= (upgrade.BuyCost))
            {
                playerCurrency -= (item.GetUpgradeSO().BuyCost);
                InstallItem(upgrade);
                upgradeStoreDisplay.UpdateInstallsDisplay(upgrade, false);
                upgradeStoreDisplay.UpdateCurrency(playerCurrency);
            }
            else
                Debug.Log("Not enough scrap");
        }
        else
            Debug.Log("No available slot for this item");
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

    bool CheckForAvailableSlot(UpgradeType upgradeType)
    {
        bool foundFreeSlot = false;
        switch (upgradeType)
        {
            case UpgradeType.Power:
                if (powerController.PowerType == null)
                    foundFreeSlot = true;
                break;
            case UpgradeType.Cargo:
                if (cargoController.CargoType == null)
                    foundFreeSlot = true;
                break;
            case UpgradeType.Shield:
                if (shieldController.ShieldType == null)
                    foundFreeSlot = true;
                break;
            case UpgradeType.Weapon:
                foreach (WeaponController weapon in weaponControllers)
                {
                    if (weapon.WeaponType == null)
                    {
                        foundFreeSlot = true;
                        break;
                    }
                }
                break;
        }
        return foundFreeSlot;
    }

    void InitializePlayerInstalledUpgradesList()
    {
        installedUpgrades.Add(powerController.PowerType);
        installedUpgrades.Add(shieldController.ShieldType);
        installedUpgrades.Add(cargoController.CargoType);
        foreach (WeaponController weapon in weaponControllers)
        {
            installedUpgrades.Add(weapon.WeaponType);
        }
    }

    void GetRegionalAvailability()
    {
        foreach (WeaponUSO upgrade in weaponUpgrades)
        {
            if (upgrade.UpgradeFaction == faction || upgrade.UpgradeFaction == Faction.Default)
            {
                availableUpgrades.Add(upgrade);
            }

        }
        foreach (CargoUSO upgrade in cargoUpgrades)
        {
            if (upgrade.UpgradeFaction == faction || upgrade.UpgradeFaction == Faction.Default)
            {
                availableUpgrades.Add(upgrade);
            }
        }
        foreach (PowerUSO upgrade in powerUpgrades)
        {
            if (upgrade.UpgradeFaction == faction || upgrade.UpgradeFaction == Faction.Default)
            {
                availableUpgrades.Add(upgrade);
            }
        }
        foreach (ShieldUSO upgrade in shieldUpgrades)
        {
            if (upgrade.UpgradeFaction == faction || upgrade.UpgradeFaction == Faction.Default)
            {
                availableUpgrades.Add(upgrade);
            }
        }
    }

    void RemoveItem(UpgradeSO upgrade)
    {
        switch (upgrade.UpgradeType)
        {
            case UpgradeType.Weapon:
                WeaponUSO item = weaponUpgrades.Where(obj => obj.name == upgrade.name).SingleOrDefault();
                for (int i = 0; i < weaponControllers.Count(); i++)
                {
                    if (weaponControllers[i].WeaponType != null)
                    {
                        if (weaponControllers[i].WeaponType.name == item.name)
                        {
                            weaponControllers[i].RemoveComponent();
                            Debug.Log("Removed weapon");
                            break;
                        }
                    }
                }
                break;
            case UpgradeType.Power:
                powerController.RemoveComponent();
                break;
            case UpgradeType.Cargo:
                cargoController.RemoveComponent();
                break;
            case UpgradeType.Shield:
                shieldController.RemoveComponent();
                break;
        }
    }

    void InstallItem(UpgradeSO upgradeItem)
    {
        switch (upgradeItem.UpgradeType)
        {
            case UpgradeType.Weapon:
                WeaponUSO weaponItem = weaponUpgrades.Where(obj => obj.name == upgradeItem.name).SingleOrDefault();
                for (int i = 0; i < weaponControllers.Count(); i++)
                {
                    if (weaponControllers[i].WeaponType == null)
                    {
                        weaponControllers[i].InstallComponent(weaponItem);
                        Debug.Log("Weapon Item Installed");
                        break;
                    }
                }
                break;
            case UpgradeType.Power:
                PowerUSO powerItem = powerUpgrades.Where(obj => obj.name == upgradeItem.name).SingleOrDefault();
                powerController.InstallComponent(powerItem);
                Debug.Log("Power Item Installed");
                break;
            case UpgradeType.Cargo:
                CargoUSO cargoItem = cargoUpgrades.Where(obj => obj.name == upgradeItem.name).SingleOrDefault();
                cargoController.InstallComponent(cargoItem);
                Debug.Log("Cargo Item Installed");
                break;
            case UpgradeType.Shield:
                ShieldUSO shieldItem = shieldUpgrades.Where(obj => obj.name == upgradeItem.name).SingleOrDefault();
                shieldController.InstallComponent(shieldItem);
                Debug.Log("Shield Item Installed");
                break;
        }
    }

    public void DisplayInfo(UpgradeItemDisplay upgradeDis)
    {
        Debug.Log("Display Called");
    }
}
