using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemDisplay : MonoBehaviour
{
    UpgradeManager upgradeManager;
    [SerializeField] Image panel;
    [SerializeField] TMP_Text upgradeNameText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Image iconImage;
    [SerializeField] Button buyButton;
    [SerializeField] Button infoButton;
    [SerializeField] UpgradeSO upgrade;

    [SerializeField] Color shieldPanelColor;
    [SerializeField] Color weaponPanelColor;
    [SerializeField] Color powerPanelColor;
    [SerializeField] Color cargoPanelColor;

    UpgradeType slotType;
    Color panelColor;

    public UpgradeType SlotType => slotType;

    private void Awake()
    {
        upgradeManager = FindAnyObjectByType<UpgradeManager>();
        if (upgrade != null) SetUpBuyDisplayTile(upgrade);
    }

    public void SetUpBuyDisplayTile(UpgradeSO upgrade)
    {
        switch (upgrade.UpgradeType)
        {
            case UpgradeType.Weapon:
                panelColor = weaponPanelColor;
                break;
            case UpgradeType.Power:
                panelColor = powerPanelColor;
                break;
            case UpgradeType.Cargo:
                panelColor = cargoPanelColor;
                break;
            case UpgradeType.Shield:
                panelColor = shieldPanelColor;
                break;
        }
        panel.color = panelColor;
        this.upgrade = upgrade;
        upgradeNameText.text = upgrade.Name;
        costText.text = "Cost: " + upgrade.BuyCost;
        iconImage.sprite = upgrade.StoreIcon;
    }

    public void SetUpSellDisplayTile(UpgradeSO upgrade)
    {
        switch (upgrade.UpgradeType)
        {
            case UpgradeType.Weapon:
                panelColor = weaponPanelColor;
                slotType = UpgradeType.Weapon;
                break;
            case UpgradeType.Power:
                panelColor = powerPanelColor;
                slotType = UpgradeType.Power;
                break;
            case UpgradeType.Cargo:
                panelColor = cargoPanelColor;
                slotType = UpgradeType.Cargo;
                break;
            case UpgradeType.Shield:
                panelColor = shieldPanelColor;
                slotType= UpgradeType.Shield;
                break;
        }
        this.upgrade = upgrade;
        upgradeNameText.text = upgrade.Name;
        costText.text = "Scrap: " + upgrade.SellCost;
        iconImage.sprite = upgrade.StoreIcon;
    }

    public void ClearSellDisplayTile()
    {
        upgradeNameText.text = "Empty Slot";
        upgrade = null;
        costText.gameObject.SetActive(false);
        iconImage.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        infoButton.gameObject.SetActive(false);
    }

    public void UpdateSellDisplayTile(UpgradeSO upgrade)
    {
        costText.gameObject.SetActive(true);
        iconImage.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(true);
        infoButton.gameObject.SetActive(true);
        this.upgrade = upgrade;
        upgradeNameText.text = upgrade.Name;
        costText.text = "Cost: " + upgrade.SellCost;
        iconImage.sprite = upgrade.StoreIcon;
    }

    public void SetBuyButtonInactive()
    {
        buyButton.interactable = false;
    }

    public void SetBuyButtonActive()
    {
        buyButton.interactable = true;
    }

    public UpgradeSO GetUpgradeSO()
    {
        return upgrade;
    }

    public void BuyItem()
    {
        upgradeManager.TryBuyItem(this);
    }

    public void SellItem()
    {
        upgradeManager.SellItem(this);
    }

    public void DisplayInformation()
    {
        upgradeManager.DisplayInfo(this);
    }
}
