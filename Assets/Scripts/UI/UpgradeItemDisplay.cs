using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemDisplay : MonoBehaviour
{
    UpgradeManager upgradeManager;
    [SerializeField] Image panel;
    [SerializeField] TMP_Text upgradeNameText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text slotTypeText;
    [SerializeField] Image iconImage;
    [SerializeField] Sprite nullImage;
    [SerializeField] Button buyButton;
    [SerializeField] Button infoButton;
    [SerializeField] UpgradeSO upgrade;

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
        panel.color = upgrade.GetStoreColor();
        this.upgrade = upgrade;
        upgradeNameText.text = upgrade.Name;
        costText.text = "Cost: " + upgrade.BuyCost;
        iconImage.sprite = upgrade.StoreIcon;
    }

    public void SetUpSellDisplayTile(UpgradeSO upgrade)
    {
        panelColor = upgrade.GetStoreColor();
        slotType = upgrade.GetUpgradeType();
        slotTypeText.text = slotType.ToString() + " Slot";
        panel.color = panelColor;
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
        iconImage.sprite = nullImage;
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
